using DND.Storage.IRepositories.Identity;
using System.Linq;
using DND.Middleware.Filter.Identity;
using DND.Middleware.Entity.Identity;
using AutoMapper;
using DND.Middleware.Identity;

namespace DND.Storage.Repositories.Identity
{
    public class RoleRepository : Repository<DatabaseContext, short, Role, RoleFilterDto>, IRoleRepository
    {
        public RoleRepository(DatabaseContext context, IAppSession session, IMapper mapper) : base(context, session, mapper)
        {
        }

        public override IQueryable<Role> Filter(IQueryable<Role> queryableSet, RoleFilterDto filter)
        {
            if (!string.IsNullOrEmpty(filter.Name))
            {
                queryableSet = queryableSet.Where(r => r.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            queryableSet = base.Filter(queryableSet, filter);
            return queryableSet;
        }
    }
}
