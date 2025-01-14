using DND.Business;
using DND.Middleware;
using DND.Middleware.Extensions;
using DND.Middleware.System;
using DND.Middleware.System.Options;
using DND.Storage;
using DND.Storage.Initializers;
using DND.Web.Server.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using DND.Middleware.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;

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
            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "DND API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
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

            builder.Services.AddScoped(options =>
            {
                var httpContextAccessor = options.GetRequiredService<IHttpContextAccessor>();
                return new AppSession
                {
                    UserId = httpContextAccessor?.HttpContext?.User.GetUserId(),
                    UserEmail = httpContextAccessor?.HttpContext?.User.GetUserEmail()
                };
            });

            //Register services dynamically
            builder.Services.RegisterServicesWithAttributes(Assembly.GetAssembly(typeof(MiddlewareAssemblyReference)));
            builder.Services.RegisterServicesWithAttributes(Assembly.GetAssembly(typeof(StorageAssemblyReference)));
            builder.Services.RegisterServicesWithAttributes(Assembly.GetAssembly(typeof(BusinessAssemblyReference)));
            builder.Services.RegisterServicesWithAttributes(Assembly.GetAssembly(typeof(WebAssemblyReference)));

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
