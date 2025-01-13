using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DND.Middleware.Attributes;
using DND.Middleware.Base.Dto;
using DND.Middleware.Dtos.Identity.RolePermissions;
using DND.Middleware.Entities.Identity;
using DND.Middleware.Exceptions;
using DND.Middleware.FilterDtos.Identity;
using DND.Middleware.System;
using DND.Storage.Repositories.Identity;

namespace DND.Business.Services.Identity
{
    public interface IRolePermissionService
    {
        Task<Result<GetRolePermissionForViewDto>> GetRolePermissionForViewAsync(int id, CancellationToken cancellationToken = default);

        Task<Result<List<GetRolePermissionForViewDto>>> GetListAsync(RolePermissionFilterDto filterDto, CancellationToken cancellationToken = default);

        Task<Result<RolePermissionDto>> CreateOrUpdateAsync(CreateOrUpdateRolePermissionDto dto, CancellationToken cancellationToken = default);

        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }

    [ScopedDependency]
    public class RolePermissionService : Service, IRolePermissionService
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;

        public RolePermissionService(IServiceProvider serviceProvider, IRolePermissionRepository rolePermissionRepository) : base(serviceProvider)
        {
            _rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<Result<GetRolePermissionForViewDto>> GetRolePermissionForViewAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = new Result<GetRolePermissionForViewDto>();
            try
            {
                var entity = await _rolePermissionRepository.GetAsSelectedAsync(id, e => new GetRolePermissionForViewDto
                {
                    RolePermission = Mapper.Map<RolePermissionDto>(e),
                    RoleName = e.Role.Name,
                    PermissionName = e.Permission.Name,
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

        public async Task<Result<List<GetRolePermissionForViewDto>>> GetListAsync(RolePermissionFilterDto filterDto, CancellationToken cancellationToken = default)
        {
            var result = new Result<List<GetRolePermissionForViewDto>>();
            try
            {
                if (!ModelState.IsValid)
                {
                    result.Error(ModelState);
                }
                else
                {
                    var entities = await _rolePermissionRepository.GetListAsSelectedAsync(filterDto, e => new GetRolePermissionForViewDto
                    {
                        RolePermission = Mapper.Map<RolePermissionDto>(e),
                        RoleName = e.Role.Name,
                        PermissionName = e.Permission.Name,
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
                    result.Success(entities);
                }
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<RolePermissionDto>> CreateOrUpdateAsync(CreateOrUpdateRolePermissionDto dto, CancellationToken cancellationToken = default)
        {
            var result = new Result<RolePermissionDto>();
            try
            {
                if (!ModelState.IsValid)
                {
                    result.Error(ModelState);
                }
                else
                {
                    RolePermissionDto rolePermissionDto;
                    if (dto.Id == null)
                    {
                        rolePermissionDto = await CreateAsync(dto, cancellationToken);
                    }
                    else
                    {
                        rolePermissionDto = await UpdateAsync(dto, cancellationToken);
                    }

                    result.Success(rolePermissionDto);
                }
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        private async Task<RolePermissionDto> CreateAsync(CreateOrUpdateRolePermissionDto dto, CancellationToken cancellationToken = default)
        {
            if (_rolePermissionRepository.IsExistingToAdd(dto))
            {
                throw new AlreadyExistException($"{nameof(RolePermission)} already exists with role id: '{dto.RoleId}' and permission id: '{dto.PermissionId}'.");
            }

            var entity = Mapper.Map<RolePermission>(dto);
            var newEntity = await _rolePermissionRepository.CreateAsync(entity, cancellationToken);
            return Mapper.Map<RolePermissionDto>(newEntity);
        }

        private async Task<RolePermissionDto> UpdateAsync(CreateOrUpdateRolePermissionDto dto, CancellationToken cancellationToken = default)
        {
            if (_rolePermissionRepository.IsExistingToUpdate(dto))
            {
                throw new AlreadyExistException($"{nameof(RolePermission)} already exists with role id: '{dto.RoleId}' and permission id: '{dto.PermissionId}'.");
            }

            var entity = await _rolePermissionRepository.UpdateAsync(dto, cancellationToken);
            return Mapper.Map<RolePermissionDto>(entity);
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = new Result<RolePermissionDto>();
            try
            {
                await _rolePermissionRepository.DeleteAsync(id, cancellationToken);
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