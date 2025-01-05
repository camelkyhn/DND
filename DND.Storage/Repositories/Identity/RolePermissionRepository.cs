using AutoMapper;
using DND.Middleware.Dtos.Identity;
using DND.Middleware.Entities.Identity;
using DND.Middleware.FilterDtos.Identity;
using DND.Middleware.Identity;
using DND.Storage.IRepositories.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DND.Middleware.Dtos.Identity.RolePermissions;

namespace DND.Storage.Repositories.Identity
{
    public class RolePermissionRepository : Repository<DatabaseContext, int, RolePermission, RolePermissionFilterDto>, IRolePermissionRepository
    {
        public RolePermissionRepository(DatabaseContext context, IAppSession session, IMapper mapper) : base(context, session, mapper)
        {
        }

        public override IQueryable<RolePermission> Filter(IQueryable<RolePermission> queryableSet, RolePermissionFilterDto filter)
        {
            if (filter.RoleId != null)
            {
                queryableSet = queryableSet.Where(rp => rp.RoleId == filter.RoleId);
            }

            if (!string.IsNullOrEmpty(filter.RoleName))
            {
                queryableSet = queryableSet.Where(rp => rp.Role.Name.ToLower().Contains(filter.RoleName.ToLower()));
            }

            if (filter.PermissionId != null)
            {
                queryableSet = queryableSet.Where(rp => rp.PermissionId == filter.PermissionId);
            }

            if (!string.IsNullOrEmpty(filter.PermissionName))
            {
                queryableSet = queryableSet.Where(rp => rp.Permission.Name.ToLower().Contains(filter.PermissionName.ToLower()));
            }

            queryableSet = base.Filter(queryableSet, filter);
            return queryableSet;
        }

        public bool IsExistingToAdd(RolePermissionDto dto)
        {
            return Context.RolePermissions.Any(rp => rp.PermissionId == dto.PermissionId && rp.RoleId == dto.RoleId);
        }

        public bool IsExistingToUpdate(RolePermissionDto dto)
        {
            return Context.RolePermissions.Any(rp => rp.Id != dto.Id && rp.PermissionId == dto.PermissionId && rp.RoleId == dto.RoleId);
        }

        public async Task<List<short>> GetRolePermissionIdListAsync(short roleId)
        {
            return await Context.RolePermissions.Where(rp => rp.RoleId == roleId).Select(rp => rp.PermissionId).ToListAsync();
        }
    }
}
