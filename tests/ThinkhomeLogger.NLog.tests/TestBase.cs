using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Runtime.InteropServices;
using ThinkhomeLogger.Abstractions;
using ThinkhomeLogger.Logging;
using Xunit;

namespace ThinkhomeLogger
{
    public abstract class TestBase
    {
        private static IServiceProvider ServiceProvider;
        private static IConfiguration Configuration;
        static TestBase()
        {

            //PreConfigureContainer((ObjectContainer.Current as AutofacObjectContainer).ContainerBuilder);    //����֧�ֳ���IOptions
            var builder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build(); 
            var services = new ServiceCollection();  
            services.AddThinkhomeRuntimeLogger(Configuration, "configs/nlog.config",  "ThinkhomeLogging"); //1. ע��nlog����
            ServiceProvider = services.BuildServiceProvider();

            ServiceProvider.AddThinkhomeLogger();    //2.�����µķ���
            //var factory = ServiceProvider.GetService<ILoggerFactory>(); 
            //factory.AddThinkhomeLogger(ServiceProvider);                       
        }
       

        public T GetService<T>()
        {
            return ServiceProvider.GetService<T>();
        }
    }
}
