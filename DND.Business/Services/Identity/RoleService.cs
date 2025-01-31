using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DND.Middleware.Attributes;
using DND.Middleware.Base.Dto;
using DND.Middleware.Dtos.Identity.Roles;
using DND.Middleware.Entities.Identity;
using DND.Middleware.FilterDtos.Identity;
using DND.Middleware.System;
using DND.Storage.Repositories.Identity;

namespace DND.Business.Services.Identity
{
    public interface IRoleService
    {
        Task<Result<GetRoleForViewDto>> GetRoleForViewAsync(short id, CancellationToken cancellationToken = default);

        Task<Result<List<GetRoleForViewDto>>> GetListAsync(RoleFilterDto filterDto, CancellationToken cancellationToken = default);

        Task<Result<RoleDto>> CreateOrUpdateAsync(CreateOrUpdateRoleDto dto, CancellationToken cancellationToken = default);

        Task<Result> DeleteAsync(short id, CancellationToken cancellationToken = default);
    }

    [ScopedDependency]
    public class RoleService : Service, IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IServiceProvider serviceProvider, IRoleRepository roleRepository) : base(serviceProvider)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Result<GetRoleForViewDto>> GetRoleForViewAsync(short id, CancellationToken cancellationToken = default)
        {
            var result = new Result<GetRoleForViewDto>();
            try
            {
                var entity = await _roleRepository.GetAsSelectedAsync(id, e => new GetRoleForViewDto
                {
                    Role = Mapper.Map<RoleDto>(e),
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

        public async Task<Result<List<GetRoleForViewDto>>> GetListAsync(RoleFilterDto filterDto, CancellationToken cancellationToken = default)
        {
            var result = new Result<List<GetRoleForViewDto>>();
            try
            {
                if (!ModelState.IsValid)
                {
                    result.Error(ModelState);
                }
                else
                {
                    var entities = await _roleRepository.GetListAsSelectedAsync(filterDto, e => new GetRoleForViewDto
                    {
                        Role = Mapper.Map<RoleDto>(e),
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

        public async Task<Result<RoleDto>> CreateOrUpdateAsync(CreateOrUpdateRoleDto dto, CancellationToken cancellationToken = default)
        {
            var result = new Result<RoleDto>();
            try
            {
                if (!ModelState.IsValid)
                {
                    result.Error(ModelState);
                }
                else
                {
                    RoleDto roleDto;
                    if (dto.Id == null)
                    {
                        roleDto = await CreateAsync(dto, cancellationToken);
                    }
                    else
                    {
                        roleDto = await UpdateAsync(dto, cancellationToken);
                    }

                    result.Success(roleDto);
                }
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        private async Task<RoleDto> CreateAsync(CreateOrUpdateRoleDto dto, CancellationToken cancellationToken = default)
        {
            var entity = Mapper.Map<Role>(dto);
            var newEntity = await _roleRepository.CreateAsync(entity, cancellationToken);
            return Mapper.Map<RoleDto>(newEntity);
        }

        private async Task<RoleDto> UpdateAsync(CreateOrUpdateRoleDto dto, CancellationToken cancellationToken = default)
        {
            var entity = await _roleRepository.UpdateAsync(dto, cancellationToken);
            return Mapper.Map<RoleDto>(entity);
        }

        public async Task<Result> DeleteAsync(short id, CancellationToken cancellationToken = default)
        {
            var result = new Result<RoleDto>();
            try
            {
                await _roleRepository.DeleteAsync(id, cancellationToken);
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