using DND.Middleware.Entities.Identity;
using DND.Middleware.FilterDtos.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using DND.Middleware.Dtos.Identity.RolePermissions;

namespace DND.Storage.IRepositories.Identity
{
    public interface IRolePermissionRepository : IRepository<int, RolePermission, RolePermissionFilterDto>
    {
        bool IsExistingToAdd(RolePermissionDto dto);
        bool IsExistingToUpdate(RolePermissionDto dto);
        Task<List<short>> GetRolePermissionIdListAsync(short roleId);
    }
}
