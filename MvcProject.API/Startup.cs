using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MvcProject.Application.Options;
using MvcProject.Infrastructure.Database;
using MvcProject.Infrastructure.Identity;
using MvcProject.Infrastructure.Options;
using System.Text;

namespace MvcProject.API
{
    public static class Startup
    {
        public static void AddAuth(this IServiceCollection services, string connectionString, IConfigurationSection jwtOptions)
        {


            services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlServer(connectionString));


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions["Issuer"],
                    ValidAudience = jwtOptions["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions["Secret"]))
                };
            })
            .AddCookie();

            services.AddCors();
            services.AddAuthorization();


            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
            });

            services
                .AddIdentity<ApplicationUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddUserManager<UserManager<ApplicationUser>>();

            services.Configure<JwtOptions>(
                jwtOptions
                );

            services.AddOptions<FileOptions>();
        }
    }
}
