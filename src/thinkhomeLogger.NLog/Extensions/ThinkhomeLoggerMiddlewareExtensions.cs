using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkhomeLogger.Logging;

namespace ThinkhomeLogger
{
    public static class ThinkhomeLoggerMiddlewareExtensions
    {
        public static IServiceProvider UseThinkhomeLogger(this IServiceProvider serviceProvider)
        {
            var factory = serviceProvider.GetService<ILoggerFactory>();
            factory.AddThinkhomeLogger(serviceProvider);                        //2.加入新的服务
            return serviceProvider;
        }
    }
}
