using AutoMapper;
using DND.Middleware.Entities.Identity;
using DND.Middleware.FilterDtos.Identity;
using System.Linq;
using DND.Middleware.Attributes;
using DND.Middleware.Web;

namespace DND.Storage.Repositories.Identity
{
    public interface IPermissionRepository : IRepository<short, Permission, PermissionFilterDto>
    {
    }

    [ScopedDependency]
    public class PermissionRepository : Repository<DatabaseContext, short, Permission, PermissionFilterDto>, IPermissionRepository
    {
        public PermissionRepository(DatabaseContext context, AppSession session, IMapper mapper) : base(context, session, mapper)
        {
        }

        public override IQueryable<Permission> Filter(IQueryable<Permission> queryableSet, PermissionFilterDto filter)
        {
            if (!string.IsNullOrEmpty(filter.Name))
            {
                queryableSet = queryableSet.Where(p => p.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            queryableSet = Filter(queryableSet, filter);
            return queryableSet;
        }
    }
}
