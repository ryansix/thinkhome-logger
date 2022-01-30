using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection; 
using Microsoft.AspNetCore.Builder;
using Linglu.Core.Logging;
using Linglu.Core.Abstractions;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static class LingluLoggerMiddlewareExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static void UseLingluLogger(this IApplicationBuilder app)
        {
            app.ApplicationServices.UseLingluLogger();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IServiceProvider UseLingluLogger(this IServiceProvider serviceProvider)
        {
            var logger= serviceProvider.GetService<ILogger<ServiceProvider>>();
            try
            {
                var opt = serviceProvider.GetService<LingluLoggerOptions>();
                var factory = serviceProvider.GetService<ILoggerFactory>(); 
                factory.AddLingluLogger(opt);                        //2.加入新的服务  
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return serviceProvider;
        }
    }
}
