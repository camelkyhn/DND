using DND.Middleware.Entities.Identity;
using DND.Middleware.FilterDtos.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DND.Middleware.Attributes;
using DND.Middleware.Dtos.Identity.UserRoles;
using DND.Middleware.Web;
using Microsoft.EntityFrameworkCore;

namespace DND.Storage.Repositories.Identity
{
    public interface IUserRoleRepository : IRepository<long, UserRole, UserRoleFilterDto>
    {
        bool IsExistingToAdd(CreateOrUpdateUserRoleDto dto);
        bool IsExistingToUpdate(CreateOrUpdateUserRoleDto dto);
        Task<List<short>> GetUserRoleIdListAsync(int userId);
    }

    [ScopedDependency]
    public class UserRoleRepository : Repository<DatabaseContext, long, UserRole, UserRoleFilterDto>, IUserRoleRepository
    {
        public UserRoleRepository(DatabaseContext context, AppSession session, IMapper mapper) : base(context, session, mapper)
        {
        }

        public override IQueryable<UserRole> Filter(IQueryable<UserRole> queryableSet, UserRoleFilterDto filter)
        {
            if (filter.UserId != null)
            {
                queryableSet = queryableSet.Where(ur => ur.UserId == filter.UserId);
            }

            if (!string.IsNullOrEmpty(filter.UserEmail))
            {
                queryableSet = queryableSet.Where(ur => ur.User.Email.ToLower().Contains(filter.UserEmail.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.RoleName))
            {
                queryableSet = queryableSet.Where(ur => ur.Role.Name.ToLower().Contains(filter.RoleName.ToLower()));
            }

            queryableSet = base.Filter(queryableSet, filter);
            return queryableSet;
        }

        public bool IsExistingToAdd(CreateOrUpdateUserRoleDto dto)
        {
            return Context.UserRoles.Any(ur => ur.UserId == dto.UserId && ur.RoleId == dto.RoleId);
        }

        public bool IsExistingToUpdate(CreateOrUpdateUserRoleDto dto)
        {
            return Context.UserRoles.Any(ur => ur.Id != dto.Id && ur.UserId == dto.UserId && ur.RoleId == dto.RoleId);
        }

        public async Task<List<short>> GetUserRoleIdListAsync(int userId)
        {
            return await Context.UserRoles.Where(ur => ur.UserId == userId).Select(ur => ur.RoleId).Distinct().ToListAsync();
        }
    }
}
