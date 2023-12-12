using Microsoft.Extensions.DependencyInjection;
using MvcProject.Application.Interfaces;
using MvcProject.Infrastructure.Options;
using MvcProject.Infrastructure.Services;

namespace MvcProject.Infrastructure.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
