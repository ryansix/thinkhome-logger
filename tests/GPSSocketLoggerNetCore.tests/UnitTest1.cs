
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ThinkHome_CommandTranslator;
using ThinkHomeDevice.Protocol;
using ThinkhomeSocketLoggerNetCore.Abstractions;
using ThinkhomeSocketLoggerNetCore.Extensions.Logging;

using Xunit;

namespace GPSSocketLoggerNetCore.tests
{
    public class UnitTest1 : TestBase
    {
        [Fact]
        public void Test1()
        {
            var logger = ServiceProvider.GetService<IThinkhomeCoordinatorLogger>();
            var message = "5A A5 00 17 10 01 10 1B E2 02 18 08 02 4B FF FE 00 00 10 62 00 00 01 32 00 00 04 CC 5B B5".Replace(" ","").ToHexBytes();
            logger.LogSend("180085300102", message);
        }


        [Fact]
        public void TestParallel()
        {
            var ts = new System.Diagnostics.Stopwatch();
            var logger = ServiceProvider.GetService<IThinkhomeCoordinatorLogger>();
            //var message = "7e02000033180085300102015b000000000000010000cb4b80040fa55c00000018123115410401040000000003020024e2080000000000000000a00400000000337e".ToHexBytes();
            var message = "7e02337e".ToHexBytes();

            var tasks = new List<Task>();
            ts.Start();
            for (int i = 0; i < 10000; i++)
            {
                var task = Task.Factory.StartNew(() =>
                {
                    logger.LogSend("180085300102", message);
                });
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
            ts.Stop();
            logger.LogSend($"ref_{(ts.ElapsedMilliseconds)}s", new byte[0]);
            logger.Dispose();
            //Thread.Sleep(15000);
        }

        [Fact]
        public void TestConcurrentAsync()
        {
            var ts = new System.Diagnostics.Stopwatch();
            var logger = ServiceProvider.GetService<IThinkhomeCoordinatorLogger>();
            var option = ServiceProvider.GetService<IOptions<NLogThinkhomeCoordinatorSetting>>();
            option.Value.IsOpen = true;
            option.Value.IsForceFinishAllQueue = true;
            //var message = "7e02000033180085300102015b000000000000010000cb4b80040fa55c00000018123115410401040000000003020024e2080000000000000000a00400000000337e".ToHexBytes();
            var message = "7e0233247e".ToHexBytes();

            var tasks = new List<Task>();
            ts.Start();
            for (int i = 0; i < 300; i++)
            {
                var task = logger.LogSendAsync("180085300102Async", message);
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
            ts.Stop();
            logger.LogSend($"async_ref_{(ts.ElapsedMilliseconds)}s", new byte[0]);
            logger.Dispose();
        }
        [Fact]
        public void TranslateCommand_Test()
        {
            var bytes = "5A A5 00 17 10 01 08 1A E1 01 21 07 00 00 FF FE 00 00 10 11 00 00 05 00 00 00 00 9F 5B B5".ToHexBytes();
            var text = CommandTranslator.TranslateCommand(BitConverter.ToString(bytes));

        }

    }
}
