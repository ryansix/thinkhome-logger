using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using ThinkhomeLogger;
using ThinkhomeLogger.Abstractions;

namespace ThinkhomeBootstrap
{
    class Program
    {
        static void Main(string[] args)
        {
            //PreConfigureContainer((ObjectContainer.Current as AutofacObjectContainer).ContainerBuilder);    //可以支持超級IOptions
            var builder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();
             
            var services = new ServiceCollection();  
            services.AddThinkhomeRuntimeLogger(configuration); //1. 注入nlog服务
            var ServiceProvider = services.BuildServiceProvider();
           

            ServiceProvider.UseThinkhomeLogger();    //2.加入新的服务

           
            var logger = ServiceProvider. GetService<ILogger<Program>>();
            logger.LogInformation("111"+"111");
        }
    }
}
