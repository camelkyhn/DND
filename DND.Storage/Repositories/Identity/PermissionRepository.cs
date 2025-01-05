using AutoMapper;
using DND.Middleware.Entities.Identity;
using DND.Middleware.FilterDtos.Identity;
using DND.Middleware.Identity;
using DND.Storage.IRepositories.Identity;
using System.Linq;

namespace DND.Storage.Repositories.Identity
{
    public class PermissionRepository : Repository<DatabaseContext, short, Permission, PermissionFilterDto>, IPermissionRepository
    {
        public PermissionRepository(DatabaseContext context, IAppSession session, IMapper mapper) : base(context, session, mapper)
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
