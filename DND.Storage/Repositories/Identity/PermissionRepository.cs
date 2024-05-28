using AutoMapper;
using DND.Middleware.Dto.Identity;
using DND.Middleware.Entity.Identity;
using DND.Middleware.Filter.Identity;
using DND.Storage.IRepositories.Identity;
using System.Linq;

namespace DND.Storage.Repositories.Identity
{
    public class PermissionRepository : Repository<DatabaseContext, short, long, Permission, PermissionDto, PermissionFilterDto>, IPermissionRepository
    {
        public PermissionRepository(DatabaseContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override IQueryable<Permission> Filter(IQueryable<Permission> queryableSet, PermissionFilterDto filter)
        {
            if (!string.IsNullOrEmpty(filter.Name))
            {
                queryableSet = queryableSet.Where(p => p.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            queryableSet = base.Filter(queryableSet, filter);
            return queryableSet;
        }
    }
}
