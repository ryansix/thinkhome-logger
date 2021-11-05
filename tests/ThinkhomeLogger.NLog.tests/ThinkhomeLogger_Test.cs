using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ThinkhomeLogger
{
    public class ThinkhomeLogger_Test : TestBase
    {
        [Fact]
        public void Test()
        {
            var logger = GetService<ILogger<ThinkhomeLogger_Test>>();
            logger.LogInformation("111");
        }
    }
}
