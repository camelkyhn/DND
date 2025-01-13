using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DND.Middleware.Attributes;
using DND.Middleware.Base.Dto;
using DND.Middleware.Dtos.Identity.Permissions;
using DND.Middleware.Entities.Identity;
using DND.Middleware.FilterDtos.Identity;
using DND.Middleware.System;
using DND.Storage.Repositories.Identity;

namespace DND.Business.Services.Identity
{
    public interface IPermissionService
    {
        Task<Result<GetPermissionForViewDto>> GetPermissionForViewAsync(short id, CancellationToken cancellationToken = default);

        Task<Result<List<GetPermissionForViewDto>>> GetListAsync(PermissionFilterDto filterDto, CancellationToken cancellationToken = default);

        Task<Result<PermissionDto>> CreateOrUpdateAsync(CreateOrUpdatePermissionDto dto, CancellationToken cancellationToken = default);

        Task<Result> DeleteAsync(short id, CancellationToken cancellationToken = default);
    }

    [ScopedDependency]
    public class PermissionService : Service, IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionService(IServiceProvider serviceProvider, IPermissionRepository permissionRepository) : base(serviceProvider)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<Result<GetPermissionForViewDto>> GetPermissionForViewAsync(short id, CancellationToken cancellationToken = default)
        {
            var result = new Result<GetPermissionForViewDto>();
            try
            {
                var entity = await _permissionRepository.GetAsSelectedAsync(id, e => new GetPermissionForViewDto
                {
                    Permission = Mapper.Map<PermissionDto>(e),
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

        public async Task<Result<List<GetPermissionForViewDto>>> GetListAsync(PermissionFilterDto filterDto, CancellationToken cancellationToken = default)
        {
            var result = new Result<List<GetPermissionForViewDto>>();
            try
            {
                if (!ModelState.IsValid)
                {
                    result.Error(ModelState);
                }
                else
                {
                    var entities = await _permissionRepository.GetListAsSelectedAsync(filterDto, e => new GetPermissionForViewDto
                    {
                        Permission = Mapper.Map<PermissionDto>(e),
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

        public async Task<Result<PermissionDto>> CreateOrUpdateAsync(CreateOrUpdatePermissionDto dto, CancellationToken cancellationToken = default)
        {
            var result = new Result<PermissionDto>();
            try
            {
                if (!ModelState.IsValid)
                {
                    result.Error(ModelState);
                }
                else
                {
                    PermissionDto permissionDto;
                    if (dto.Id == null)
                    {
                        permissionDto = await CreateAsync(dto, cancellationToken);
                    }
                    else
                    {
                        permissionDto = await UpdateAsync(dto, cancellationToken);
                    }

                    result.Success(permissionDto);
                }
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        private async Task<PermissionDto> CreateAsync(CreateOrUpdatePermissionDto dto, CancellationToken cancellationToken = default)
        {
            var entity = Mapper.Map<Permission>(dto);
            var newEntity = await _permissionRepository.CreateAsync(entity, cancellationToken);
            return Mapper.Map<PermissionDto>(newEntity);
        }

        private async Task<PermissionDto> UpdateAsync(CreateOrUpdatePermissionDto dto, CancellationToken cancellationToken = default)
        {
            var entity = await _permissionRepository.UpdateAsync(dto, cancellationToken);
            return Mapper.Map<PermissionDto>(entity);
        }

        public async Task<Result> DeleteAsync(short id, CancellationToken cancellationToken = default)
        {
            var result = new Result<PermissionDto>();
            try
            {
                await _permissionRepository.DeleteAsync(id, cancellationToken);
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