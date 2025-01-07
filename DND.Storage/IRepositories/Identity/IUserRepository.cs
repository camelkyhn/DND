using DND.Middleware.Entities.Identity;
using DND.Middleware.FilterDtos.Identity;
using System.Threading.Tasks;

namespace DND.Storage.IRepositories.Identity
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
    }
}
