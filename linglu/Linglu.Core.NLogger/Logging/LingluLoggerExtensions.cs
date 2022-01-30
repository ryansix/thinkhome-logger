using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linglu.Core.Abstractions;

namespace Linglu.Core.Logging
{
   public static class LingluLoggerExtensions
    {
        public static ILoggerFactory AddLingluLogger(this ILoggerFactory factory, LingluLoggerOptions opt)
        {
            factory.AddProvider(new LingluLoggerProvider(opt)); 
            return factory;
        }
    }
}
