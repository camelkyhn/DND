using DND.Middleware.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using DND.Middleware.Entities.Identity;
using DND.Middleware.FilterDtos.Identity;
using AutoMapper;
using DND.Middleware.Attributes;
using DND.Middleware.Dtos.Identity.Accounts;
using DND.Middleware.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DND.Storage.Repositories.Identity
{
    public interface IUserRepository : IRepository<int, User, UserFilterDto>
    {
        /// <summary>
        /// Get the <see cref="User"/> entity by searching the email.
        /// </summary>
        /// <param name="email">A string value to search</param>
        /// <returns>Returns the <see cref="User"/> by searching email.</returns>
        /// <exception cref="NotFoundException">If the search result is null.</exception>
        Task<User> GetByEmailAsync(string email);
        Task IncreaseFailedAttemptsAsync(User user);
        Task ClearFailedAttemptsAsync(User user);
        bool IsEmailTaken(string email);
        bool IsEmailTaken(string email, int userId);
        Task<User> RegisterAsync(RegisterDto dto);
    }

    [ScopedDependency]
    public class UserRepository : Repository<DatabaseContext, int, User, UserFilterDto>, IUserRepository
    {
        public UserRepository(DatabaseContext context, AppSession session, IMapper mapper) : base(context, session, mapper)
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
                throw new NotFoundException($"{nameof(User)} is not found with email '{email}'.");
            }

            return user;
        }

        public async Task IncreaseFailedAttemptsAsync(User user)
        {
            user.LastModificationTime = DateTime.UtcNow;
            user.AccessFailedCount++;
            Context.Entry(user).Property(u => u.LastModificationTime).IsModified = true;
            Context.Entry(user).Property(u => u.AccessFailedCount).IsModified = true;
            if (user.AccessFailedCount >= 5)
            {
                user.LockoutEnd = DateTime.UtcNow.AddMinutes(5 * (user.AccessFailedCount - 4));
                Context.Entry(user).Property(u => u.LockoutEnd).IsModified = true;
            }

            await Context.SaveChangesAsync();
        }

        public async Task ClearFailedAttemptsAsync(User user)
        {
            user.LastModificationTime = DateTime.UtcNow;
            user.AccessFailedCount = 0;
            user.LockoutEnd = null;
            await Context.SaveChangesAsync();
        }

        public bool IsEmailTaken(string email)
        {
            return Context.Users.Any(u => u.Email.ToLower() == email.ToLower());
        }

        public bool IsEmailTaken(string email, int userId)
        {
            return Context.Users.Any(u => u.Id != userId && u.Email.ToLower() == email.ToLower());
        }

        public async Task<User> RegisterAsync(RegisterDto dto)
        {
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                IsLockoutEnabled = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, dto.Password);
            return await CreateAsync(user);
        }
    }
}
