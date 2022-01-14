using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LingluLogger.Abstractions
{
    public class LingluAuditLoggerOptions 
    {
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
    }
}
