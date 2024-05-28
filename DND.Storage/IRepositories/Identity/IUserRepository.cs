using DND.Middleware.Dto.Identity;
using DND.Middleware.Entity.Identity;
using DND.Middleware.Filter.Identity;
using System.Threading.Tasks;

namespace DND.Storage.IRepositories.Identity
{
    public interface IUserRepository : IRepository<long, long, User, UserDto, UserFilterDto>
    {
        Task<User> GetByEmailAsync(string email);
        Task IncreaseFailedAttemptsAsync(User user);
        Task ClearFailedAttemptsAsync(User user);
        bool IsEmailTaken(string email);
        bool IsEmailTaken(string email, long userId);
    }
}
