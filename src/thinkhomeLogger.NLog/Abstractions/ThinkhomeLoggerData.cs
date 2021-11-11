using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkhomeLogger.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    internal class ThinkhomeLoggerData
    {
        public ThinkhomeLoggerData()
        {
            MacAddress = ThinkhomeNetwork.GetMACAddress();
            IpAddress = ThinkhomeNetwork.GetIPAddress();
            InstanceName = ThinkhomeNetwork.GetComputerName();
        }

        public ThinkhomeLoggerData(ThinkhomeLoggerOptions options) : this()
        {
            if (options == null) return;    //如果配置文件没有配置，则使用默认对象
            this.AppId = options?.AppId;
            this.ProgramId = options?.ProgramId;
            if (!string.IsNullOrWhiteSpace(options.IPAddress))
                this.IpAddress = options.IPAddress;

        }
        public ThinkhomeLoggerData(ThinkhomeLoggerOptions options, LogLevel logLevel) : this(options)
        {
            this.Level = logLevel.Ordinal.ToString();
        }
        /// <summary>
        /// 应用程序标识 
        /// </summary>
        [JsonProperty("appId")]
        public string AppId { get;private set; }
        /// <summary>
        /// 程序类型，可空
        /// </summary>
        [JsonProperty("programId")]
        public string ProgramId { get;private set; }
        /// <summary>
        /// 本地计算机的 NetBIOS 名称
        /// </summary>
        [JsonProperty("instanceName")]
        public string InstanceName { get;private set; }
        /// <summary>
        /// 时间
        /// </summary>
        [JsonProperty("time")]
        public string Time { get;private set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        /// <summary>
        /// 日志源
        /// </summary>
        [JsonProperty("Source")]
        public string Source { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; } = "";
        /// <summary>
        /// 日志等级枚举值 2-调试 3-信息 4-警告 5-错误
        /// </summary>
        [JsonProperty("level")]
        public string Level { get; private set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }
        /// <summary>
        /// //服务器内网IP
        /// </summary>
        [JsonProperty("ipAddress")]
        public string IpAddress { get;private set; }
        /// <summary>
        /// mac 地址
        /// </summary>
        public string MacAddress { get;private set; }
    }
}
