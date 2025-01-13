using System.Linq;
using DND.Middleware.Entities.Identity;
using DND.Middleware.FilterDtos.Identity;
using AutoMapper;
using DND.Middleware.Attributes;
using DND.Middleware.Web;

namespace DND.Storage.Repositories.Identity
{
    public interface IRoleRepository : IRepository<short, Role, RoleFilterDto>
    {
    }

    [ScopedDependency]
    public class RoleRepository : Repository<DatabaseContext, short, Role, RoleFilterDto>, IRoleRepository
    {
        public RoleRepository(DatabaseContext context, AppSession session, IMapper mapper) : base(context, session, mapper)
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
