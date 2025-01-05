using DND.Middleware.Exceptions;
using DND.Storage;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Options;
using DND.Middleware.System;
using DND.Business.IServices.Identity;
using DND.Middleware.ResultDtos.Identity;
using DND.Middleware.Constants;
using DND.Middleware.Entities.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;
using DND.Middleware.Dtos.Identity.Accounts;
using DND.Middleware.System.Options;

namespace DND.Business.Services.Identity
{
    public class AccountService : Service, IAccountService
    {
        private readonly AuthorizationOptions _authorizationOptions;

        public AccountService(IRepositoryContext repositoryContext, IOptions<AuthorizationOptions> options) : base(repositoryContext)
        {
            _authorizationOptions = options.Value;
        }

        public async Task<Result<LoginResultDto>> LoginAsync(LoginDto dto)
        {
            var result = new Result<LoginResultDto>();
            try
            {
                var userResult = await _repositoryContext.Users.GetByEmailAsync(dto.Email);
                if (!userResult.IsEmailConfirmed)
                {
                    return new Result<LoginResultDto> { ErrorMessage = Errors.LoginConfirmEmail };
                }

                if (userResult.IsLockoutEnabled && userResult.LockoutEnd != null && userResult.LockoutEnd > DateTime.UtcNow)
                {
                    return new Result<LoginResultDto> { ErrorMessage = $"{Errors.AccountLockedOut}{userResult.LockoutEnd:yyyy-MM-dd HH:mm:ss}" };
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
                            new Claim(Claims.Id, userResult.Id.ToString()),
                            new Claim(Claims.Email, userResult.Email)
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
                        await _repositoryContext.Users.ClearFailedAttemptsAsync(userResult);
                    }
                }
                else
                {
                    if (userResult.IsLockoutEnabled)
                    {
                        await _repositoryContext.Users.IncreaseFailedAttemptsAsync(userResult);
                        if (userResult.LockoutEnd != null)
                        {
                            return new Result<LoginResultDto> { ErrorMessage = $"{Errors.AccountLockedOut}{userResult.LockoutEnd:yyyy-MM-dd HH:mm:ss}" };
                        }
                    }

                    return new Result<LoginResultDto> { ErrorMessage = Errors.WrongLoginAttempt };
                }
            }
            catch (NotFoundException)
            {
                return new Result<LoginResultDto> { ErrorMessage = Errors.NotFoundEmail };
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }
    }
}
