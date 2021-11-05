using System;
using System.Collections.Generic;
using System.Text;

namespace ThinkhomeSocketLoggerNetCore.Abstractions
{
    public class NLogThinkhomeCoordinatorSetting
    {
        public NLogThinkhomeCoordinatorSetting()
        {
            MaxLogPenddingRecordCount = 200;
        }
        public long MaxLogPenddingRecordCount { get; set; }
        public bool IsOpen { get; set; }
        /// <summary>
        /// 是否強制完成所有
        /// </summary>
        public bool IsForceFinishAllQueue { get; set; }
        public Func<byte[], string> Callback { get; set; }
    }
}
