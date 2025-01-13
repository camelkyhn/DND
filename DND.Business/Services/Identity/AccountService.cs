using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Options;
using DND.Middleware.System;
using DND.Middleware.ResultDtos.Identity;
using DND.Middleware.Constants;
using DND.Middleware.Entities.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DND.Business.Services.Notification;
using DND.Middleware.Attributes;
using DND.Middleware.Dtos.Identity.Accounts;
using DND.Middleware.Dtos.Identity.UserRoles;
using DND.Middleware.Dtos.Notification.Mail;
using DND.Middleware.System.Options;
using DND.Storage.Repositories.Identity;

namespace DND.Business.Services.Identity
{
    public interface IAccountService
    {
        Task<Result<LoginResultDto>> LoginAsync(LoginDto dto);
        Task<Result> RegisterAsync(RegisterDto dto);
        Task<Result> ConfirmEmailAsync(ConfirmEmailDto dto);
        Task<Result> ForgotPasswordAsync(ForgotPasswordDto dto);
        Task<Result> ResetPasswordAsync(ResetPasswordDto dto);
        Task<Result> ChangePasswordAsync(ChangePasswordDto dto);
    }

    [ScopedDependency]
    public class AccountService : Service, IAccountService
    {
        private readonly AuthorizationOptions _authorizationOptions;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMailService _mailService;

        public AccountService(IServiceProvider serviceProvider,
            IOptions<AuthorizationOptions> options,
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            IRoleRepository uroleRepository,
            IMailService mailService) : base(serviceProvider)
        {
            _authorizationOptions = options.Value;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = uroleRepository;
            _mailService = mailService;
        }

        public async Task<Result<LoginResultDto>> LoginAsync(LoginDto dto)
        {
            var result = new Result<LoginResultDto>();
            try
            {
                if (!ModelState.IsValid)
                {
                    return result.Error(ModelState);
                }

                var userResult = await _userRepository.GetByEmailAsync(dto.Email);
                if (!userResult.IsEmailConfirmed)
                {
                    return result.Error(Errors.LoginConfirmEmail);
                }

                if (userResult.IsLockoutEnabled && userResult.LockoutEnd != null && userResult.LockoutEnd > DateTime.UtcNow)
                {
                    return result.Error($"{Errors.AccountLockedOut}{userResult.LockoutEnd:yyyy-MM-dd HH:mm:ss}");
                }

                var hasher = new PasswordHasher<User>();
                var hashResult = hasher.VerifyHashedPassword(userResult, userResult.PasswordHash, dto.Password);
                if (hashResult == PasswordVerificationResult.Success)
                {
                    var key = Encoding.ASCII.GetBytes(_authorizationOptions.Jwt.Key);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new List<Claim>
                        {
                            new(Claims.Id, userResult.Id.ToString()),
                            new(Claims.Email, userResult.Email)
                        }),
                        Expires = DateTime.UtcNow.AddDays(1),
                        Issuer = _authorizationOptions.Jwt.Issuer,
                        Audience = _authorizationOptions.Jwt.Audience,
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    result.Success(new LoginResultDto { AccessToken = tokenHandler.WriteToken(token) });
                    if (userResult.LockoutEnd != null || userResult.AccessFailedCount > 0)
                    {
                        await _userRepository.ClearFailedAttemptsAsync(userResult);
                    }
                }
                else
                {
                    if (userResult.IsLockoutEnabled)
                    {
                        await _userRepository.IncreaseFailedAttemptsAsync(userResult);
                        if (userResult.LockoutEnd != null)
                        {
                            return result.Error($"{Errors.AccountLockedOut}{userResult.LockoutEnd:yyyy-MM-dd HH:mm:ss}");
                        }
                    }

                    return result.Error(Errors.WrongLoginAttempt);
                }
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result> RegisterAsync(RegisterDto dto)
        {
            var result = new Result();
            try
            {
                if (!ModelState.IsValid)
                {
                    return result.Error(ModelState);
                }

                if (IsPasswordFormatValid(dto.Password))
                {
                    return result.Error(Errors.Password);
                }

                if (_userRepository.IsEmailTaken(dto.Email))
                {
                    return result.Error(Errors.EmailTaken);
                }

                var createdUser = await _userRepository.RegisterAsync(dto);
                var token = GenerateToken(createdUser.SecurityStamp);
                var callbackUrl = Urls.ConfirmEmail + "?userId=" + createdUser.Id + "&token=" + token;
                var mail = new MailDto
                {
                    ToEmailAddressList = new List<string> { createdUser.Email },
                    Subject = Titles.EmailConfirmation,
                    Message = Messages.EmailConfirmation(callbackUrl),
                    IsBodyPlainText = false
                };
                await _mailService.SendMailAsync(mail);
                result.Success();
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result> ConfirmEmailAsync(ConfirmEmailDto dto)
        {
            var result = new Result();
            try
            {
                if (!ModelState.IsValid)
                {
                    return result.Error(ModelState);
                }

                var userResult = await _userRepository.GetAsync(dto.UserId);
                if (userResult.IsEmailConfirmed)
                {
                    return result.Error(Errors.AlreadyConfirmedEmail);
                }

                if (!IsConfirmationTokenCorrect(dto.Token, userResult.SecurityStamp))
                {
                    return result.Error(Errors.ExpiredToken);
                }

                userResult.IsEmailConfirmed = true;
                await _userRepository.UpdateAsync(userResult);
                var memberRole = await _roleRepository.FirstOrDefaultAsync(r => r.Name == Entities.Roles.Member);
                await _userRoleRepository.CreateAsync(new CreateOrUpdateUserRoleDto
                {
                    UserId = userResult.Id,
                    RoleId = memberRole.Id
                });
                result.Success();
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var result = new Result();
            try
            {
                if (!ModelState.IsValid)
                {
                    return result.Error(ModelState);
                }

                var userResult = await _userRepository.GetByEmailAsync(dto.Email);
                if (!userResult.IsEmailConfirmed)
                {
                    return result.Error(Errors.ResetPasswordConfirmEmail);
                }

                var token = GenerateToken(userResult.SecurityStamp);
                var callbackUrl = Urls.ResetPassword + "?userId=" + userResult.Id + "&token=" + token;
                var mail = new MailDto
                {
                    ToEmailAddressList = new List<string> { userResult.Email },
                    Subject = Titles.ResetPassword,
                    Message = Messages.ResetPassword(callbackUrl),
                    IsBodyPlainText = false
                };
                await _mailService.SendMailAsync(mail);
                result.Success();
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var result = new Result();
            try
            {
                if (!ModelState.IsValid)
                {
                    return result.Error(ModelState);
                }

                var userResult = await _userRepository.GetAsync(dto.UserId);
                if (!IsConfirmationTokenCorrect(dto.Token, userResult.SecurityStamp))
                {
                    return result.Error(Errors.ExpiredToken);
                }

                var hasher = new PasswordHasher<User>();
                userResult.PasswordHash = hasher.HashPassword(userResult, dto.Password);
                userResult.SecurityStamp = Guid.NewGuid().ToString();
                await _userRepository.UpdateAsync(userResult);
                result.Success();
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result> ChangePasswordAsync(ChangePasswordDto dto)
        {
            var result = new Result();
            try
            {
                if (!ModelState.IsValid)
                {
                    return result.Error(ModelState);
                }

                if (IsPasswordFormatValid(dto.NewPassword))
                {
                    return result.Error(Errors.Password);
                }

                var userResult = await _userRepository.GetAsync(AppSession.UserId.GetValueOrDefault());
                var hasher = new PasswordHasher<User>();
                var hashResult = hasher.VerifyHashedPassword(userResult, userResult.PasswordHash, dto.CurrentPassword);
                if (hashResult != PasswordVerificationResult.Success)
                {
                    return result.Error(Errors.CurrentPassword);
                }

                userResult.PasswordHash = hasher.HashPassword(userResult, dto.NewPassword);
                await _userRepository.UpdateAsync(userResult);
                result.Success();
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        private static bool IsPasswordFormatValid(string password)
        {
            return password.Any(char.IsLower) && password.Any(char.IsUpper) && password.Any(char.IsDigit) && password.Any(char.IsSymbol);
        }

        private static string GenerateToken(string securityStamp)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes(securityStamp))));
        }

        private static bool IsConfirmationTokenCorrect(string token, string securityStamp)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(Encoding.UTF8.GetString(Convert.FromBase64String(token)))) == securityStamp;
        }
    }
}
