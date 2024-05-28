using DND.Middleware.Dto.Identity;
using DND.Middleware.Exceptions;
using DND.Storage.IRepositories.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DND.Middleware.Filter.Identity;
using DND.Middleware.Entity.Identity;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace DND.Storage.Repositories.Identity
{
    public class UserRepository : Repository<DatabaseContext, long, long, User, UserDto, UserFilterDto>, IUserRepository
    {
        public UserRepository(DatabaseContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override IQueryable<User> Filter(IQueryable<User> queryableSet, UserFilterDto filter)
        {
            if (!string.IsNullOrEmpty(filter.Email))
            {
                queryableSet = queryableSet.Where(u => u.Email.ToLower().Contains(filter.Email.ToLower()));
            }

            if (filter.IsEmailConfirmed != null)
            {
                queryableSet = queryableSet.Where(u => u.IsEmailConfirmed == filter.IsEmailConfirmed);
            }

            if (filter.IsEmailEnabled != null)
            {
                queryableSet = queryableSet.Where(u => u.IsEmailEnabled == filter.IsEmailEnabled);
            }

            queryableSet = base.Filter(queryableSet, filter);
            return queryableSet;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await Context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (user == null)
            {
                throw new NotFoundException(nameof(User));
            }

            return user;
        }

        public override async Task<User> CreateAsync(UserDto dto, CancellationToken cancellationToken = default)
        {
            dto.SecurityStamp = Guid.NewGuid().ToString();
            return await base.CreateAsync(dto, cancellationToken);
        }

        public override async Task<User> UpdateAsync(UserDto dto, CancellationToken cancellationToken = default)
        {
            dto.SecurityStamp = Guid.NewGuid().ToString();
            return await base.UpdateAsync(dto, cancellationToken);
        }

        public async Task IncreaseFailedAttemptsAsync(User user)
        {
            user.ModificationTime = DateTimeOffset.UtcNow;
            user.AccessFailedCount++;
            Context.Entry(user).Property(u => u.ModificationTime).IsModified = true;
            Context.Entry(user).Property(u => u.AccessFailedCount).IsModified = true;
            if (user.AccessFailedCount >= 5)
            {
                user.LockoutEnd = DateTimeOffset.UtcNow.AddMinutes(5 * (user.AccessFailedCount - 4));
                Context.Entry(user).Property(u => u.LockoutEnd).IsModified = true;
            }

            await Context.SaveChangesAsync();
        }

        public async Task ClearFailedAttemptsAsync(User user)
        {
            user.ModificationTime = DateTimeOffset.UtcNow;
            user.AccessFailedCount = 0;
            user.LockoutEnd = null;
            Context.Entry(user).Property(u => u.ModificationTime).IsModified = true;
            Context.Entry(user).Property(u => u.AccessFailedCount).IsModified = true;
            Context.Entry(user).Property(u => u.LockoutEnd).IsModified = true;
            await Context.SaveChangesAsync();
        }

        public bool IsEmailTaken(string email)
        {
            return Context.Users.Any(u => u.Email.ToLower() == email.ToLower());
        }

        public bool IsEmailTaken(string email, long userId)
        {
            return Context.Users.Any(u => u.Id != userId && u.Email.ToLower() == email.ToLower());
        }
    }
}
