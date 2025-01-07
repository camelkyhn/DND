using AutoMapper;
using DND.Middleware.Extensions;
using DND.Middleware.Identity;
using DND.Middleware.System;
using DND.Middleware.System.Options;
using DND.Storage;
using DND.Storage.Initializers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DND.Business.Services.Identity;

namespace DND.Web.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<ConnectionStringsOptions>(builder.Configuration.GetSection(ConnectionStringsOptions.ConnectionStrings));
            builder.Services.Configure<AuthorizationOptions>(builder.Configuration.GetSection(AuthorizationOptions.Authorization));
            builder.Services.Configure<AdministrationOptions>(builder.Configuration.GetSection(AdministrationOptions.Administration));
            builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection(SmtpOptions.Smtp));

            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var authorizationOptions = builder.Configuration.GetAuthorizationOptions();
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authorizationOptions.Jwt.Issuer,
                    ValidAudience = authorizationOptions.Jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authorizationOptions.Jwt.Key)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

            builder.Services.AddDbContext<DatabaseContext>(db => db.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection")));

            builder.Services.AddAutoMapper(typeof(MapperProfile));

            builder.Services.AddScoped<IAppSession>(options =>
            {
                var httpContextAccessor = options.GetRequiredService<IHttpContextAccessor>();
                var session = new AppSession
                {
                    UserId = httpContextAccessor.HttpContext?.User.GetUserId(),
                    UserEmail = httpContextAccessor.HttpContext?.User.GetUserEmail()
                };
                return session;
            });

            builder.Services.AddScoped<IRepositoryContext>(options =>
            {
                var dbContext = options.GetRequiredService<DatabaseContext>();
                var mapper = options.GetRequiredService<IMapper>();
                var session = options.GetRequiredService<IAppSession>();
                return new RepositoryContext(dbContext, mapper, session);
            });
            builder.Services.AddScoped<IUserService, UserService>();

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(name: "AreaRoute", pattern: "{area:exists}/{controller=Account}/{action=Login}/{id?}");

            // Migrate and seed the database
            new DatabaseInitializer().InitializeDatabase(app);

            app.Run();
        }
    }
}
