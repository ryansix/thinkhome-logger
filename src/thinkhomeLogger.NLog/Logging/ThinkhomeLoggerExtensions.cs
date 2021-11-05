using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkhomeLogger.Abstractions;

namespace ThinkhomeLogger.Logging
{
   public static class ThinkhomeLoggerExtensions
    {
        public static ILoggerFactory AddThinkhomeLogger(this ILoggerFactory factory,  IServiceProvider serviceProvider)
        {
            var opt = serviceProvider.GetService<IOptions<ThinkhomeLoggerOptions>>();
            factory.AddProvider(new ThinkhomeLoggerProvider(opt.Value)); 
            return factory;
        }
    }
}
