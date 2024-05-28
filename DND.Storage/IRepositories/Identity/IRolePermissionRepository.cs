using DND.Middleware.Dto.Identity;
using DND.Middleware.Entity.Identity;
using DND.Middleware.Filter.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DND.Storage.IRepositories.Identity
{
    public interface IRolePermissionRepository : IRepository<int, long, RolePermission, RolePermissionDto, RolePermissionFilterDto>
    {
        bool IsExistingToAdd(RolePermissionDto dto);
        bool IsExistingToUpdate(RolePermissionDto dto);
        Task<List<short>> GetRolePermissionIdListAsync(short roleId);
    }
}
