 
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions; 
using NLog.ThinkhomeSocketLoggerNetCore.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using ThinkhomeSocketLoggerNetCore.Abstractions;
using ThinkhomeSocketLoggerNetCore.Extensions.Logging;

namespace NLog.ThinkhomeSocketLoggerNetCore.Extensions
{
    public static class NLogGPSSocketLoggerNetCoreExtensions
    {
        public static IServiceCollection AddNLogThinkhomeCoordinatorLogger(this IServiceCollection services,Action<NLogThinkhomeCoordinatorSetting> action)
        {
            services.Configure(action);
            services.Replace(new ServiceDescriptor(typeof(IThinkhomeCoordinatorLogger), typeof(NLogThinkhomeCoordinatorLogger), ServiceLifetime.Singleton));
            return services;
        }
    }
}
