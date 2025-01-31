using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DND.Middleware.Attributes;
using DND.Middleware.Base.Dto;
using DND.Middleware.Dtos.Identity.UserRoles;
using DND.Middleware.Entities.Identity;
using DND.Middleware.Exceptions;
using DND.Middleware.FilterDtos.Identity;
using DND.Middleware.System;
using DND.Storage.Repositories.Identity;

namespace DND.Business.Services.Identity
{
    public interface IUserRoleService
    {
        Task<Result<GetUserRoleForViewDto>> GetUserRoleForViewAsync(long id, CancellationToken cancellationToken = default);

        Task<Result<List<GetUserRoleForViewDto>>> GetListAsync(UserRoleFilterDto filterDto, CancellationToken cancellationToken = default);

        Task<Result<UserRoleDto>> CreateOrUpdateAsync(CreateOrUpdateUserRoleDto dto, CancellationToken cancellationToken = default);

        Task<Result> DeleteAsync(long id, CancellationToken cancellationToken = default);
    }

    [ScopedDependency]
    public class UserRoleService : Service, IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleService(IServiceProvider serviceProvider, IUserRoleRepository userRoleRepository) : base(serviceProvider)
        {
            _userRoleRepository = userRoleRepository;
        }

        public async Task<Result<GetUserRoleForViewDto>> GetUserRoleForViewAsync(long id, CancellationToken cancellationToken = default)
        {
            var result = new Result<GetUserRoleForViewDto>();
            try
            {
                var entity = await _userRoleRepository.GetAsSelectedAsync(id, e => new GetUserRoleForViewDto
                {
                    UserRole = Mapper.Map<UserRoleDto>(e),
                    UserFirstName = e.User.FirstName,
                    UserLastName = e.User.LastName,
                    UserName = e.User.UserName,
                    UserEmail = e.User.Email,
                    RoleName = e.Role.Name,
                    Creator = new EntityAuditorForViewDto
                    {
                        Id = e.CreatorUserId,
                        FirstName = e.CreatorUser.FirstName,
                        LastName = e.CreatorUser.LastName,
                        FullName = e.CreatorUserId != null ? e.CreatorUser.FirstName + " " + e.CreatorUser.LastName : null,
                        UserName = e.CreatorUser.UserName,
                        Email = e.CreatorUser.Email,
                        AuditedTime = e.CreationTime
                    },
                    LastModifier = new EntityAuditorForViewDto
                    {
                        Id = e.LastModifierUserId ?? e.CreatorUserId,
                        FirstName = e.LastModifierUser != null ? e.LastModifierUser.FirstName : e.CreatorUser.FirstName,
                        LastName = e.LastModifierUser != null ? e.LastModifierUser.LastName : e.CreatorUser.LastName,
                        FullName = e.LastModifierUser != null
                            ? e.LastModifierUser.FirstName + " " + e.LastModifierUser.LastName
                            : e.CreatorUserId != null ? e.CreatorUser.FirstName + " " + e.CreatorUser.LastName : null,
                        UserName = e.LastModifierUser != null ? e.LastModifierUser.UserName : e.CreatorUser.UserName,
                        Email = e.LastModifierUser != null ? e.LastModifierUser.Email : e.CreatorUser.Email,
                        AuditedTime = e.LastModificationTime ?? e.CreationTime
                    },
                    IsCreatedByCurrentUser = e.CreatorUserId == AppSession.UserId.GetValueOrDefault()
                }, cancellationToken);
                result.Success(entity);
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<List<GetUserRoleForViewDto>>> GetListAsync(UserRoleFilterDto filterDto, CancellationToken cancellationToken = default)
        {
            var result = new Result<List<GetUserRoleForViewDto>>();
            try
            {
                if (!ModelState.IsValid)
                {
                    result.Error(ModelState);
                }
                else
                {
                    var entities = await _userRoleRepository.GetListAsSelectedAsync(filterDto, e => new GetUserRoleForViewDto
                    {
                        UserRole = Mapper.Map<UserRoleDto>(e),
                        UserFirstName = e.User.FirstName,
                        UserLastName = e.User.LastName,
                        UserName = e.User.UserName,
                        UserEmail = e.User.Email,
                        RoleName = e.Role.Name,
                        Creator = new EntityAuditorForViewDto
                        {
                            Id = e.CreatorUserId,
                            FirstName = e.CreatorUser.FirstName,
                            LastName = e.CreatorUser.LastName,
                            FullName = e.CreatorUserId != null ? e.CreatorUser.FirstName + " " + e.CreatorUser.LastName : null,
                            UserName = e.CreatorUser.UserName,
                            Email = e.CreatorUser.Email,
                            AuditedTime = e.CreationTime
                        },
                        LastModifier = new EntityAuditorForViewDto
                        {
                            Id = e.LastModifierUserId ?? e.CreatorUserId,
                            FirstName = e.LastModifierUser != null ? e.LastModifierUser.FirstName : e.CreatorUser.FirstName,
                            LastName = e.LastModifierUser != null ? e.LastModifierUser.LastName : e.CreatorUser.LastName,
                            FullName = e.LastModifierUser != null
                                ? e.LastModifierUser.FirstName + " " + e.LastModifierUser.LastName
                                : e.CreatorUserId != null ? e.CreatorUser.FirstName + " " + e.CreatorUser.LastName : null,
                            UserName = e.LastModifierUser != null ? e.LastModifierUser.UserName : e.CreatorUser.UserName,
                            Email = e.LastModifierUser != null ? e.LastModifierUser.Email : e.CreatorUser.Email,
                            AuditedTime = e.LastModificationTime ?? e.CreationTime
                        },
                        IsCreatedByCurrentUser = e.CreatorUserId == AppSession.UserId.GetValueOrDefault()
                    }, cancellationToken);
                    result.Success(entities, filterDto);
                }
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<UserRoleDto>> CreateOrUpdateAsync(CreateOrUpdateUserRoleDto dto, CancellationToken cancellationToken = default)
        {
            var result = new Result<UserRoleDto>();
            try
            {
                if (!ModelState.IsValid)
                {
                    result.Error(ModelState);
                }
                else
                {
                    UserRoleDto userRoleDto;
                    if (dto.Id == null)
                    {
                        userRoleDto = await CreateAsync(dto, cancellationToken);
                    }
                    else
                    {
                        userRoleDto = await UpdateAsync(dto, cancellationToken);
                    }

                    result.Success(userRoleDto);
                }
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        private async Task<UserRoleDto> CreateAsync(CreateOrUpdateUserRoleDto dto, CancellationToken cancellationToken = default)
        {
            if (_userRoleRepository.IsExistingToAdd(dto))
            {
                throw new AlreadyExistException($"{nameof(UserRole)} already exists with user id: '{dto.UserId}' and role id: '{dto.RoleId}'.");
            }

            var entity = Mapper.Map<UserRole>(dto);
            var newEntity = await _userRoleRepository.CreateAsync(entity, cancellationToken);
            return Mapper.Map<UserRoleDto>(newEntity);
        }

        private async Task<UserRoleDto> UpdateAsync(CreateOrUpdateUserRoleDto dto, CancellationToken cancellationToken = default)
        {
            if (_userRoleRepository.IsExistingToUpdate(dto))
            {
                throw new AlreadyExistException($"{nameof(UserRole)} already exists with user id: '{dto.UserId}' and role id: '{dto.RoleId}'.");
            }

            var entity = await _userRoleRepository.UpdateAsync(dto, cancellationToken);
            return Mapper.Map<UserRoleDto>(entity);
        }

        public async Task<Result> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var result = new Result<UserRoleDto>();
            try
            {
                await _userRoleRepository.DeleteAsync(id, cancellationToken);
                result.Success();
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }
    }
}