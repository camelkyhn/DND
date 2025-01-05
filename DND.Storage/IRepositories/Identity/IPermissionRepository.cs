using DND.Middleware.Entities.Identity;
using DND.Middleware.FilterDtos.Identity;

namespace DND.Storage.IRepositories.Identity
{
    public interface IPermissionRepository : IRepository<short, Permission, PermissionFilterDto>
    {
    }
}
