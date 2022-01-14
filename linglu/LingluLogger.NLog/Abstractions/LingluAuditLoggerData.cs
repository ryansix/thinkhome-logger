
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LingluLogger.Abstractions
{
    public class LingluAuditLoggerData //: LingluLoggerData
    {
        //public LingluAuditLoggerData() : base() { }
        //public LingluAuditLoggerData(LingluLoggerOptions options) : base(options)
        //{
        //}
        //public LingluAuditLoggerData(LingluLoggerOptions options, LogLevel logLevel) : this(options)
        //{
        //}

        public LingluAuditLoggerData()
        {
            MacAddress = LingluNetwork.GetMACAddress();
            IpAddress = LingluNetwork.GetIPAddress();
            InstanceName = LingluNetwork.GetComputerName();
        }

        public LingluAuditLoggerData(LingluLoggerOptions options) : this()
        {
            if (options == null) return;    //如果配置文件没有配置，则使用默认对象
            this.AppId = options?.AppId;
            this.ProgramId = options?.ProgramId;
            if (!string.IsNullOrWhiteSpace(options.IPAddress))
                this.IpAddress = options.IPAddress;
        }
        public LingluAuditLoggerData(LingluLoggerOptions options, LogLevel logLevel) : this(options)
        {
            this.Level = logLevel.Ordinal.ToString();
        }

        [JsonPropertyName("logGroup")]
        public string LogGroup { get; set; }
        [JsonPropertyName("account")]
        public string AccountId { get; set; }

        [JsonPropertyName("clientID")]
        public string ClientId { get; set; }

        [JsonPropertyName("clientModel")]
        public string ClientModel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("clientSys")]
        public string ClientSys { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("appVer")]
        public string AppVer { get; set; }
        [JsonPropertyName("controllerName")]
        public string ControllerName { get; set; }

        [JsonPropertyName("controllerDescribe")]
        public string ControllerDescribe { get; set; }

        [JsonPropertyName("actionName")]
        public string ActionName { get; set; }

        [JsonPropertyName("requestStartTime")]
        public DateTime RequestStartTime { get; set; }

        [JsonPropertyName("requestEndTime")]
        public DateTime RequestEndTime { get; set; }

        [JsonPropertyName("inParameter")]
        public string InParameter { get; set; }

        [JsonPropertyName("outParameter")]
        public string OutParameter { get; set; }

        /// <summary>
        /// 响应码编号
        /// </summary>
        [JsonPropertyName("resultCode")]
        public string ResultCode { get; set; }
        /// <summary>
        /// 响应码描述
        /// </summary>
        [JsonPropertyName("resultContent")]
        public string ResultContent { get; set; }


        /// <summary>
        /// 应用程序标识 
        /// </summary>
        [JsonPropertyName("appId")]
        public string AppId { get; private set; }
        /// <summary>
        /// 程序类型，可空
        /// </summary>
        [JsonPropertyName("programId")]
        public string ProgramId { get; private set; }
        /// <summary>
        /// 本地计算机的 NetBIOS 名称
        /// </summary>
        [JsonPropertyName("instanceName")]
        public string InstanceName { get; private set; }
        /// <summary>
        /// //服务器内网IP
        /// </summary>
        [JsonPropertyName("ipAddress")]
        public string IpAddress { get;   set; }
        /// <summary>
        /// mac 地址
        /// </summary>
        public  string MacAddress { get;   set; }
        /// <summary>
        /// 日志等级枚举值 2-调试 3-信息 4-警告 5-错误
        /// </summary>
        [JsonPropertyName("level")]
        public string Level { get; private set; }
    }
}
