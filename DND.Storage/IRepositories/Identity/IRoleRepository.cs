using DND.Middleware.Entities.Identity;
using DND.Middleware.FilterDtos.Identity;

namespace DND.Storage.IRepositories.Identity
{
    public interface IRoleRepository : IRepository<short, Role, RoleFilterDto>
    {
    }
}
