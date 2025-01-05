using DND.Middleware.System.Options;
using Microsoft.Extensions.Configuration;

namespace DND.Middleware.Extensions
{
    public static class Configurations
    {
        public static AuthorizationOptions GetAuthorizationOptions(this IConfiguration configuration)
        {
            var section = configuration.GetSection(AuthorizationOptions.Authorization);
            return section.Get<AuthorizationOptions>();
        }
    }
}
