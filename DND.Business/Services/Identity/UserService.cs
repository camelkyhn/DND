﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DND.Middleware.Attributes;
using DND.Middleware.Base.Dto;
using DND.Middleware.Dtos.Identity.Users;
using DND.Middleware.Entities.Identity;
using DND.Middleware.FilterDtos.Identity;
using DND.Middleware.System;
using DND.Storage.Repositories.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DND.Business.Services.Identity
{
    public interface IUserService
    {
        Task<Result<GetUserForViewDto>> GetUserForViewAsync(int id, CancellationToken cancellationToken = default);

        Task<Result<List<GetUserForViewDto>>> GetListAsync(UserFilterDto filterDto, CancellationToken cancellationToken = default);

        Task<Result<UserDto>> CreateOrUpdateAsync(CreateOrUpdateUserDto dto, CancellationToken cancellationToken = default);

        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }

    [ScopedDependency]
    public class UserService : Service, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IServiceProvider serviceProvider, IUserRepository userRepository) : base(serviceProvider)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<GetUserForViewDto>> GetUserForViewAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = new Result<GetUserForViewDto>();
            try
            {
                var entity = await _userRepository.GetAsSelectedAsync(id, e => new GetUserForViewDto
                {
                    User = Mapper.Map<UserDto>(e),
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

        public async Task<Result<List<GetUserForViewDto>>> GetListAsync(UserFilterDto filterDto, CancellationToken cancellationToken = default)
        {
            var result = new Result<List<GetUserForViewDto>>();
            try
            {
                if (!ModelState.IsValid)
                {
                    result.Error(ModelState);
                }
                else
                {
                    var entities = await _userRepository.GetListAsSelectedAsync(filterDto, e => new GetUserForViewDto
                    {
                        User = Mapper.Map<UserDto>(e),
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

        public async Task<Result<UserDto>> CreateOrUpdateAsync(CreateOrUpdateUserDto dto, CancellationToken cancellationToken = default)
        {
            var result = new Result<UserDto>();
            try
            {
                if (!ModelState.IsValid)
                {
                    result.Error(ModelState);
                }
                else
                {
                    UserDto userDto;
                    if (dto.Id == null)
                    {
                        userDto = await CreateAsync(dto, cancellationToken);
                    }
                    else
                    {
                        userDto = await UpdateAsync(dto, cancellationToken);
                    }

                    result.Success(userDto);
                }
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        private async Task<UserDto> CreateAsync(CreateOrUpdateUserDto dto, CancellationToken cancellationToken = default)
        {
            var entity = Mapper.Map<User>(dto);
            entity.PasswordHash = new PasswordHasher<User>().HashPassword(entity, dto.Password);
            entity.SecurityStamp = Guid.NewGuid().ToString();
            var newEntity = await _userRepository.CreateAsync(entity, cancellationToken);
            return Mapper.Map<UserDto>(newEntity);
        }

        private async Task<UserDto> UpdateAsync(CreateOrUpdateUserDto dto, CancellationToken cancellationToken = default)
        {
            var entity = await _userRepository.UpdateAsync(dto, cancellationToken);
            return Mapper.Map<UserDto>(entity);
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = new Result<UserDto>();
            try
            {
                await _userRepository.DeleteAsync(id, cancellationToken);
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