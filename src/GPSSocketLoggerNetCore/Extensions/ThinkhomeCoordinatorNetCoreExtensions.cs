using ThinkhomeSocketLoggerNetCore.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThinkhomeSocketLoggerNetCore.Extensions
{
    public static class ThinkhomeCoordinatorNetCoreExtensions
    {
        public static IServiceCollection AddThinkhomeLogger(this IServiceCollection services)
        {
            services.AddSingleton<IThinkhomeCoordinatorLogger, DefaultThinkhomeCoordinatorLogger>(); 
            return services;
        }
    }
}
