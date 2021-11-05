﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkhomeLogger.Abstractions;

namespace ThinkhomeLogger
{
    public static class ThinkhomeLoggingServiceExtensions
    {
        public static void AddThinkhomeRuntimeLogger(this IServiceCollection services, IConfiguration configuration, string configfile, string selection)
        {
            services.AddLogging(builder => builder.AddThinkhomeConfigureLogging(configfile, configuration, selection)); //1. 注入nlog服务
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="configfile"></param>
        /// <param name="startupAction"></param>
        public static void AddThinkhomeRuntimeLogger(this IServiceCollection services,  string configfile, Action<ThinkhomeLoggerOptions> startupAction)
        {
            services.AddLogging(builder => builder.AddThinkhomeConfigureLogging(configfile, startupAction)); //1. 注入nlog服务
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="configfile"></param>
        public static void AddThinkhomeRuntimeLogger(this IServiceCollection services, IConfiguration configuration, string configfile)
        {
            services.AddThinkhomeRuntimeLogger(configuration, configfile, "ThinkhomeLogging"); //1. 注入nlog服务
        }

        public static void AddThinkhomeRuntimeLogger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddThinkhomeRuntimeLogger(configuration, "configs/nlog.config", "ThinkhomeLogging"); //1. 注入nlog服务
        }

      
    }
    internal static class LoggingBuilderExtensions
    {

        internal static void AddThinkhomeConfigureLogging(this ILoggingBuilder builder, string configfile, Action<ThinkhomeLoggerOptions> startupAction)
        {
            builder.Services.Configure(startupAction);
            NLog.LogManager.LoadConfiguration(configfile);
            builder.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            builder.SetMinimumLevel(LogLevel.Trace);
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
            builder.AddThinkhomeConfigureLogging(configfile, opt=> configuration.GetSection(selection).Get<ThinkhomeLoggerOptions>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <param name="selection"></param>
        public static void AddThinkhomeConfigureLogging(this ILoggingBuilder builder, IConfiguration configuration, string selection)
        {
            builder.AddThinkhomeConfigureLogging("log.config", configuration, selection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        public static void AddThinkhomeConfigureLogging(this ILoggingBuilder builder, IConfiguration configuration)
        {
            builder.AddThinkhomeConfigureLogging("log.config", configuration, "ThinkhomeLogging");
        }

    }
}
