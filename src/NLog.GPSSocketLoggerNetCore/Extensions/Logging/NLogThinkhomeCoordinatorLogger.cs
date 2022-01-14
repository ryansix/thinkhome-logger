
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks; 
using System.Linq;
using ThinkhomeSocketLoggerNetCore.Extensions.Logging;
using ThinkhomeSocketLoggerNetCore.Abstractions;
using System.Net.NetworkInformation;

namespace NLog.ThinkhomeSocketLoggerNetCore.Extensions.Logging
{

    public class NLogThinkhomeCoordinatorLogger : IThinkhomeCoordinatorLogger, IDisposable
    {
        private readonly ILogger _logger;
        private readonly ILogger<NLogThinkhomeCoordinatorLogger> _sysLogger;
        private readonly ConcurrentQueue<LogEventInfo> _logQueue = new ConcurrentQueue<LogEventInfo>();
        private readonly NLogThinkhomeCoordinatorSetting _option;

        public NLogThinkhomeCoordinatorLogger(ILogger<NLogThinkhomeCoordinatorLogger> logger, IOptions<NLogThinkhomeCoordinatorSetting> options)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _sysLogger = logger;
            _option = options?.Value;
        }

        public void Dispose()
        {
            if (!_option.IsForceFinishAllQueue) return;
            ForceFinishAllQueue();
        }
        /// <summary>
        /// 強制性執行完
        /// </summary>
        private void ForceFinishAllQueue()
        {
            if (!_logQueue.IsEmpty)
            {
                _sysLogger.LogInformation($" 強制寫掉數據!{_logQueue.Count}");
                WriteLogWork();
            }
        }
        #region 同步執行
        public void LogReceive(string terminalNo, byte[] message)
        {
            if (!_option.IsOpen) return;
            if (!EnsureMaxLogPenddingRecordCount()) return;
            var logEventInfo = CreateLogEventInfo(terminalNo);
            logEventInfo.Properties["msgtype"] = "rec>> ";
            var messageHexString = BitConverter.ToString(message).Replace("-", " ");
            logEventInfo.Message = messageHexString;//message.ToHexString();
            if (_option.Callback != null)
                logEventInfo.Properties["msgtext"] = _option.Callback(message);
            _logQueue.Enqueue(logEventInfo);
            TryEnterLog();
        }

        public void LogSend(string terminalPhoneNo, byte[] message)
        {
            if (!_option.IsOpen) return;
            if (!EnsureMaxLogPenddingRecordCount()) return;
            var logEventInfo = CreateLogEventInfo(terminalPhoneNo);
            logEventInfo.Properties["msgtype"] = "send>> ";
            var messageHexString = BitConverter.ToString(message).Replace("-", " ");
            logEventInfo.Message = messageHexString;//message.ToHexString();
            if (_option.Callback != null)
                logEventInfo.Properties["msgtext"] = _option.Callback(message);
            _logQueue.Enqueue(logEventInfo);
            TryEnterLog();
        }
        #endregion

        public Task LogReceiveAsync(string terminalPhoneNo, byte[] message)
        {
            //if (!_option.IsOpen) return Task.CompletedTask;
            //if (!EnsureMaxLogPenddingRecordCount()) return Task.CompletedTask;
            //var logEventInfo = CreateLogEventInfo(terminalPhoneNo);
            //logEventInfo.Properties["msgtype"] = "rec>> ";
            //var messageHexString = BitConverter.ToString(message).Replace("-", " ");
            //logEventInfo.Message = messageHexString;//message.ToHexString();
            //if (_option.Callback != null)
            //    logEventInfo.Properties["msgtext"] = _option.Callback(message);
            //_logQueue.Enqueue(logEventInfo);
            //TryEnterLogAsync();
            //return Task.CompletedTask;
            return Task.Factory.StartNew(() => { LogReceive(terminalPhoneNo, message); });
        }
        public Task LogSendAsync(string terminalPhoneNo, byte[] message)
        { 
            return Task.Factory.StartNew(() => { LogSend(terminalPhoneNo, message); });
        }

        private bool EnsureMaxLogPenddingRecordCount()
        {
            if (_logQueue.Count > _option.MaxLogPenddingRecordCount)
            {
                var logEventInfo = CreateLogEventInfo("MaxLogPenddingRecordCountWarmming");
                logEventInfo.Message = $"數量已經超過:{_logQueue.Count - _option.MaxLogPenddingRecordCount}";
                _logQueue.Enqueue(logEventInfo);
                return false;
            }
            return true;
        }



        private int _logWriting;
        /// <summary>
        /// true:value滿足  舊value==0 && value可以更改成1
        /// false:反之
        /// </summary>
        /// <returns></returns>
        private bool EnterLogWriting()
        {
            return Interlocked.CompareExchange(ref _logWriting, 1, 0) == 0;
        }
        private void ExitLogWriting()
        {
            Interlocked.Exchange(ref _logWriting, 0);
        }
        private Task TryEnterLogAsync()
        {
            if (!EnterLogWriting()) return Task.CompletedTask;
            return Task.Factory.StartNew(() =>
            {
                WriteLogWork();
            });
        }
        private void TryEnterLog()
        {
            if (!EnterLogWriting()) return;
            WriteLogWork();
        }


        private void WriteLogWork()
        {
            while (_logQueue.Count > 0)
            {
                LogEventInfo logEventInfo;
                if (_logQueue.TryDequeue(out logEventInfo))
                {
                    if (logEventInfo != null)
                        _logger.Log(logEventInfo);
                }
            }
            ExitLogWriting();
        }

        internal static LogEventInfo CreateLogEventInfo()
        {
            var logEventInfo = LogEventInfo.Create(NLog.LogLevel.Info, typeof(NLogThinkhomeCoordinatorLogger).FullName, string.Empty);
            logEventInfo.Properties["msgtype"] = "default";
            logEventInfo.Properties["mac"] = "LHAC" + GetMACIp().Replace("-","");
            return logEventInfo;
        }
        internal static LogEventInfo CreateLogEventInfo(string terminalNo)
        {
            var logEventInfo = CreateLogEventInfo();
            logEventInfo.Properties["terminalPhoneNo"] = terminalNo;
            logEventInfo.Properties["mac"] = "LHAC" + GetMACIp().Replace("-", "");
            return logEventInfo;
        }

        public void ChangeSetting(NLogThinkhomeCoordinatorSetting setting)
        {
            _option.IsForceFinishAllQueue = setting.IsForceFinishAllQueue;
            _option.IsOpen = setting.IsOpen;
            _option.MaxLogPenddingRecordCount = setting.MaxLogPenddingRecordCount;
            //return Task.CompletedTask;
        }
        private static string macAddress = "00-00-00-00-00-00";

        /// <summary>
        /// 获取电脑 MAC（物理） 地址
        /// </summary>
        /// <returns></returns>
        private static string GetMACIp()
        {
            if (!string.IsNullOrEmpty(macAddress) &&
                !macAddress.StartsWith("00-00-00")) 
                return macAddress;  
            var bytes = GetMacAddress().GetAddressBytes();
            macAddress = BitConverter.ToString(bytes); ;
            return macAddress;
        }


        /// <summary>
        /// Gets the MAC address of the current PC.
        /// </summary>
        /// <returns></returns>
        private static PhysicalAddress GetMacAddress()
        {
            var nic = GetNetworkInterface();
            if (nic != null)
            {
                return nic.GetPhysicalAddress();
            }
            return new PhysicalAddress(new byte[] { 0, 0, 0, 0, 0, 0 });
        }
        private static NetworkInterface GetNetworkInterface()
        {
            // order interfaces by speed and filter out down and loopback
            // take first of the remaining
            var interfaces = NetworkInterface.GetAllNetworkInterfaces().ToList(); 

            if (interfaces == null || !interfaces.Any())
                return null;

            //.OrderByDescending(c => c.Speed)
            var firstUpInterface = interfaces.
                FirstOrDefault(c => c.NetworkInterfaceType != NetworkInterfaceType.Loopback
                    && c.OperationalStatus == OperationalStatus.Up
                    && c.Name.ToLower().Contains("eth")
                    && (!c.Description.ToLower().Contains("virtu"))
                    && (!c.Description.Contains("Pseudo")));

            return firstUpInterface;
        }
    }
}
