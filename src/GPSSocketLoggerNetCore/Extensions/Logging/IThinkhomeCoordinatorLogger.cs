using ThinkhomeSocketLoggerNetCore.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ThinkhomeSocketLoggerNetCore.Extensions.Logging
{
    public interface IThinkhomeCoordinatorLogger : IDisposable
    {
        void LogSend(string terminalNo, byte[] message);
        void LogReceive(string terminalNo, byte[] message);

        Task LogSendAsync(string terminalNo, byte[] message);
        Task LogReceiveAsync(string terminalNo, byte[] message);
        void ChangeSetting(NLogThinkhomeCoordinatorSetting setting);
    }
}
