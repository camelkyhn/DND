using DND.Middleware.Dto.Identity;
using DND.Middleware.Entity.Identity;
using DND.Middleware.Filter.Identity;

namespace DND.Storage.IRepositories.Identity
{
    public interface IPermissionRepository : IRepository<short, long, Permission, PermissionDto, PermissionFilterDto>
    {
    }
}
