using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThinkhomeSocketLoggerNetCore.Abstractions;
using Microsoft.Extensions.Logging; 

namespace ThinkhomeSocketLoggerNetCore.Extensions.Logging
{
    public class DefaultThinkhomeCoordinatorLogger : IThinkhomeCoordinatorLogger
    {
        private readonly ILogger _logger;
        public DefaultThinkhomeCoordinatorLogger(ILogger<DefaultThinkhomeCoordinatorLogger> logger)
        {
            _logger = logger;
        }

        public void ChangeSetting(NLogThinkhomeCoordinatorSetting setting)
        {
             
        }

        public void Dispose()
        {
             
        }

        public void LogReceive(string terminalPhoneNo, byte[] message)
        {
            _logger.LogInformation($"receive: {terminalPhoneNo} >> " + BitConverter.ToString(message));
        }

        public Task LogReceiveAsync(string terminalPhoneNo, byte[] message)
        {
            return Task.CompletedTask;
        }

        public void LogSend(string terminalPhoneNo, byte[] message)
        {
            _logger.LogInformation($" send>> {terminalPhoneNo} >> " + BitConverter.ToString(message));
        }

        public Task LogSendAsync(string terminalPhoneNo, byte[] message)
        {
            return Task.CompletedTask;
        }
    }
}
