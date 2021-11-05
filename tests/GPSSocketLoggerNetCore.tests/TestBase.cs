using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Extensions.Logging;
using NLog.ThinkhomeSocketLoggerNetCore.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using ThinkHome_CommandTranslator;
using ThinkhomeSocketLoggerNetCore.Extensions;

namespace GPSSocketLoggerNetCore.tests
{
    public class TestBase
    {
        public static IServiceProvider ServiceProvider;
        static TestBase()
        {

            //PreConfigureContainer((ObjectContainer.Current as AutofacObjectContainer).ContainerBuilder);    //可以支持超級IOptions
            var serviceHostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    //config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                })
                .ConfigureLogging((hostContext, configureLogging) =>
                {

                    NLog.LogManager.LoadConfiguration("configs/nlog.config");

                    configureLogging.AddNLog(new NLogProviderOptions
                    {
                        CaptureMessageTemplates = true,
                        CaptureMessageProperties = true
                    });
                })

                .ConfigureServices((hostContext, services) =>
                {
                    services.AddThinkhomeLogger();
                    services.AddNLogThinkhomeCoordinatorLogger(option =>
                    {
                        option.MaxLogPenddingRecordCount = 20000;
                        option.IsOpen = true;
                        option.Callback = (bytes) =>
                        {
                            return CommandTranslator.TranslateCommand(BitConverter.ToString(bytes));
                        };
                    });
                });
            var host = serviceHostBuilder.Build();
            host.Start();
            ServiceProvider = host.Services;

        }

 
    }
}
