using Microsoft.Extensions.DependencyInjection;
using MvcProject.Application.Interfaces;
using MvcProject.Infrastructure.Options;
using MvcProject.Infrastructure.Services;
using System.Reflection;
using System;

namespace MvcProject.Infrastructure.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }


        public static object GetFieldValue<T>(this T @this, string fieldName)
        {
            Type type = @this.GetType();
            FieldInfo field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            return field.GetValue(@this);
        }
    }
}
