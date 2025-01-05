using DND.Middleware.System.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DND.Storage.Initializers
{
    public class DatabaseInitializer
    {
        private DatabaseContext _dbContext;
        private AdministrationOptions _administrationOptions;

        public void InitializeDatabase(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            _dbContext.IsHistoryDisabled = true;
            _dbContext.Database.Migrate();

            var administrationOptions = scope.ServiceProvider.GetRequiredService<IOptions<AdministrationOptions>>();
            _administrationOptions = administrationOptions.Value;
            new IdentitySeeder(_dbContext, _administrationOptions).Seed();
        }
    }
}
