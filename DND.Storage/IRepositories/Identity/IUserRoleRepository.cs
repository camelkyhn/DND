using DND.Middleware.Dto.Identity;
using DND.Middleware.Entity.Identity;
using DND.Middleware.Filter.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DND.Storage.IRepositories.Identity
{
    public interface IUserRoleRepository : IRepository<long, UserRole, UserRoleFilterDto>
    {
        bool IsExistingToAdd(UserRoleDto dto);
        bool IsExistingToUpdate(UserRoleDto dto);
        Task<List<short>> GetUserRoleIdListAsync(int userId);
    }
}
