using DND.Middleware.Entities.Identity;
using DND.Middleware.FilterDtos.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using DND.Middleware.Dtos.Identity.UserRoles;

namespace DND.Storage.IRepositories.Identity
{
    public interface IUserRoleRepository : IRepository<long, UserRole, UserRoleFilterDto>
    {
        bool IsExistingToAdd(UserRoleDto dto);
        bool IsExistingToUpdate(UserRoleDto dto);
        Task<List<short>> GetUserRoleIdListAsync(int userId);
    }
}
