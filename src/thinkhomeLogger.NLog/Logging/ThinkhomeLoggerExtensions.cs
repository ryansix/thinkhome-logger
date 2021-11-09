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
        public static ILoggerFactory AddThinkhomeLogger(this ILoggerFactory factory, ThinkhomeLoggerOptions opt)
        { 
            //var opt = serviceProvider.GetService<IOptions<ThinkhomeLoggerOptions>>()?.Value;
            factory.AddProvider(new ThinkhomeLoggerProvider(opt)); 
            return factory;
        }
    }
}
