using DND.Middleware.Dto.Identity;
using DND.Middleware.Entity.Identity;
using DND.Middleware.Filter.Identity;

namespace DND.Storage.IRepositories.Identity
{
    public interface IRoleRepository : IRepository<short, long, Role, RoleDto, RoleFilterDto>
    {
    }
}
