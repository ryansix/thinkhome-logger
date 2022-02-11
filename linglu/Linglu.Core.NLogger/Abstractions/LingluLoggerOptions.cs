 
using System;
namespace Linglu.Core.Abstractions
{
    /// <summary>
    /// 主程序配置
    /// </summary>
    public class LingluLoggerOptions
    {
        /// <summary>
        /// 地址
        /// </summary>
        public string IPAddress { get; set; }
        /// <summary>
        /// 应用程序标识
        /// </summary> 
        public string AppId { get; set; }= "Application";
        /// <summary>
        /// 程序类型，可空
        /// </summary> 
        public string ProgramId { get; set; } = "ProgramId";
        /// <summary>
        /// 日志源
        /// </summary> 
        public string Source { get; set; }  
    }
}
