﻿using Microsoft.Extensions.DependencyInjection;
using MvcProject.Application.Interfaces;
using MvcProject.Infrastructure.Identity;
using MvcProject.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Infrastructure
{
    public static class DependencyInjectionExtensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICategoryService, CategoryService>();
        }
    }
}
