using DND.Middleware.Dto.Identity;
using DND.Storage.IRepositories.Identity;
using System.Linq;
using DND.Middleware.Filter.Identity;
using DND.Middleware.Entity.Identity;
using AutoMapper;

namespace DND.Storage.Repositories.Identity
{
    public class RoleRepository : Repository<DatabaseContext, short, long, Role, RoleDto, RoleFilterDto>, IRoleRepository
    {
        public RoleRepository(DatabaseContext context, IMapper mapper) : base(context, mapper)
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
