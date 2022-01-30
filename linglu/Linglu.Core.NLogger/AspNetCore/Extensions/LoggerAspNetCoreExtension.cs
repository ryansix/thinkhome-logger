
using Linglu.Core.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linglu.Core.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public static class LoggerAspNetCoreExtension
    {
        public static IServiceCollection AddNLogLoggerAspNetCore(this IServiceCollection services, IConfiguration configuration, string configfile, string selection)
        {
            services.AddLingluRuntimeLogger(configuration, configfile, selection);
            return services;
        }
        public static void AddNLogLoggerAspNetCore(this IServiceCollection services, string configfile, Func<IServiceProvider, LingluLoggerOptions> startupAction)
        {
            services.AddLingluRuntimeLogger(configfile, startupAction);
        }
        public static void AddNLogLoggerAspNetCore(this IServiceCollection services, IConfiguration configuration, string configfile)
        {
            services.AddLingluRuntimeLogger(configuration, configfile);
        }

        public static void AddNLogLoggerAspNetCore(this IServiceCollection services, IConfiguration configuration) {
            services.AddLingluRuntimeLogger(configuration);
        }

        

    }
}
