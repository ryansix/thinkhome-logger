using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkhomeLogger.Logging;
using ThinkhomeLogger.Abstractions;
using Microsoft.Extensions.Options;

namespace ThinkhomeLogger
{
    /// <summary>
    /// 
    /// </summary>
    public static class ThinkhomeLoggerMiddlewareExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IServiceProvider UseThinkhomeLogger(this IServiceProvider serviceProvider)
        {
            var logger= serviceProvider.GetService<ILogger<ServiceProvider>>();
            try
            {
                var opt = serviceProvider.GetService<ThinkhomeLoggerOptions>();
                var factory = serviceProvider.GetService<ILoggerFactory>();
                factory.AddThinkhomeLogger(opt);                        //2.加入新的服务
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return serviceProvider;
        }
    }
}
