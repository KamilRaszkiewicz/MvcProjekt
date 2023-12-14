using Microsoft.Extensions.DependencyInjection;
using MvcProject.Application.Interfaces;
using MvcProject.Application.Options;
using MvcProject.Application.Services;

namespace MvcProject.Application.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IBorrowingService, BorrowingService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IBorrowForWishListService, BorrowForWishListService>();
            services.AddScoped<IWishlistService, WishlistService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddSingleton<IPaginationService, PaginationService>();

            services.AddOptions<JwtOptions>();
            services.AddOptions<PaginationOptions>();
        }
    }
}
