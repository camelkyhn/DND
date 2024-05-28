using DND.Middleware.Dto.Identity;
using DND.Storage.IRepositories.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DND.Middleware.Entity.Identity;
using DND.Middleware.Filter.Identity;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace DND.Storage.Repositories.Identity
{
    public class UserRoleRepository : Repository<DatabaseContext, int, long, UserRole, UserRoleDto, UserRoleFilterDto>, IUserRoleRepository
    {
        public UserRoleRepository(DatabaseContext context, IMapper mapper) : base(context, mapper)
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

        public bool IsExistingToAdd(UserRoleDto dto)
        {
            return Context.UserRoles.Any(ur => ur.UserId == dto.UserId && ur.RoleId == dto.RoleId);
        }

        public bool IsExistingToUpdate(UserRoleDto dto)
        {
            return Context.UserRoles.Any(ur => ur.Id != dto.Id && ur.UserId == dto.UserId && ur.RoleId == dto.RoleId);
        }

        public async Task<List<short>> GetUserRoleIdListAsync(long userId)
        {
            return await Context.UserRoles.Where(ur => ur.UserId == userId).Select(ur => ur.RoleId).ToListAsync();
        }
    }
}
