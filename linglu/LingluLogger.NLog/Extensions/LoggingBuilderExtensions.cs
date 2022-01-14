using LingluLogger.Abstractions;
using Microsoft.Extensions.Configuration; 
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using LingluLogger;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class LingluLoggingServiceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="configfile"></param>
        /// <param name="selection"></param>
        public static void AddLingluRuntimeLogger(this IServiceCollection services, IConfiguration configuration, string configfile, string selection)
        { 
            services.AddLogging(builder => builder.AddThinkhomeConfigureLogging(configfile,configuration,selection)); //1. 注入nlog服务
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configfile"></param>
        /// <param name="startupAction"></param>
        public static void AddLingluRuntimeLogger(this IServiceCollection services,  string configfile, Func<IServiceProvider, LingluLoggerOptions> startupAction)
        { 
            services.AddLogging(builder => builder.AddLingluConfigureLogging(configfile, startupAction)); //1. 注入nlog服务
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="configfile"></param>
        public static void AddLingluRuntimeLogger(this IServiceCollection services, IConfiguration configuration, string configfile)
        {
            services.AddLingluRuntimeLogger(configuration, configfile, "LingluLogging"); //1. 注入nlog服务
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddLingluRuntimeLogger(this IServiceCollection services, IConfiguration configuration)
        {  
            services.AddLingluRuntimeLogger(configuration, "nlog.config", "LingluLogging"); //1. 注入nlog服务
        }
    }
    internal static class LoggingBuilderExtensions
    {
        internal static void AddLingluConfigureLogging(this ILoggingBuilder builder, string configfile, Func<IServiceProvider, LingluLoggerOptions> thinkhomeLoggerOptionsFactory)
        {
            builder.ClearProviders();
            builder.Services.AddSingleton(thinkhomeLoggerOptionsFactory);
            NLog.LogManager.LoadConfiguration(configfile);
            builder.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Trace);
            builder.Services.AddTransient<ILingluNLogService, LingluNLogService>();

        }
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configfile"></param>
        /// <param name="configuration"></param>
        /// <param name="selection"></param>
        internal static void AddThinkhomeConfigureLogging(this ILoggingBuilder builder, string configfile, IConfiguration configuration, string selection)
        {
            builder.AddLingluConfigureLogging(configfile, (sp =>
            {
                var opt = configuration.GetSection(selection).Get<LingluLoggerOptions>(); 
                return opt;
            } 
           ));
        } 
    }
}
