using NLog;
using System; 
using System.Text.Json.Serialization; 
namespace Linglu.Core.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    internal class LingluLoggerData
    {
        public LingluLoggerData()
        {
            MacAddress = LingluNetwork.GetMACAddress();
            IpAddress = LingluNetwork.GetIPAddress();
            InstanceName = LingluNetwork.GetComputerName();
        }

        public LingluLoggerData(LingluLoggerOptions options) : this()
        {
            if (options == null) return;    //如果配置文件没有配置，则使用默认对象
            this.AppId = options?.AppId;
            this.ProgramId = options?.ProgramId;
            if (!string.IsNullOrWhiteSpace(options.IPAddress))
                this.IpAddress = options.IPAddress;
        }
        public LingluLoggerData(LingluLoggerOptions options, LogLevel logLevel) : this(options)
        {
            this.Level = logLevel.Ordinal.ToString();
        }
        /// <summary>
        /// 应用程序标识 
        /// </summary>
        [JsonPropertyName("appId")]
        public string AppId { get;private set; }
        /// <summary>
        /// 程序类型，可空
        /// </summary>
        [JsonPropertyName("programId")]
        public string ProgramId { get;private set; }
        /// <summary>
        /// 本地计算机的 NetBIOS 名称
        /// </summary>
        [JsonPropertyName("instanceName")]
        public string InstanceName { get;private set; }
        /// <summary>
        /// 时间
        /// </summary>
        [JsonPropertyName("time")]
        public string Time { get;private set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        /// <summary>
        /// 日志源
        /// </summary>
        [JsonPropertyName("Source")]
        public string Source { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [JsonPropertyName("subject")]
        public string Subject { get; set; } = "";
        /// <summary>
        /// 日志等级枚举值 2-调试 3-信息 4-警告 5-错误
        /// </summary>
        [JsonPropertyName("level")]
        public string Level { get; private set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        [JsonPropertyName("content")]
        public string Content { get; set; }
        /// <summary>
        /// //服务器内网IP
        /// </summary>
        [JsonPropertyName("ipAddress")]
        public string IpAddress { get;private set; }
        /// <summary>
        /// mac 地址
        /// </summary>
        public string MacAddress { get;private set; }
    }
}
