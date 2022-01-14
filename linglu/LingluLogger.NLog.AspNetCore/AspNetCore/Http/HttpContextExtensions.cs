using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpContextExtensions
    {
        public static string GetClientIP(this HttpContext context, bool tryUseXForwardHeader = true)
        {
            try
            {
                string ip = null;
                if (tryUseXForwardHeader)
                    ip = SplitCsv(GetHeaderValueAs<string>(context, "X-Forwarded-For")).FirstOrDefault();

                if (string.IsNullOrWhiteSpace(ip) && context?.Connection?.RemoteIpAddress != null)
                    ip = context.Connection.RemoteIpAddress.ToString();

                if (string.IsNullOrWhiteSpace(ip))
                    ip = GetHeaderValueAs<string>(context, "REMOTE_ADDR");

                if (string.IsNullOrWhiteSpace(ip))
                    throw new Exception("Unable to determine caller's IP.");

                ip = ip == "::1" ? "127.0.0.1" : ip;

                return ip;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        private static List<string> SplitCsv(string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable<string>()
                .Select(s => s.Trim())
                .ToList();
        }
        private static T GetHeaderValueAs<T>(HttpContext context, string headerName)
        {
            var values = new StringValues();

            if (context?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();

                if (!string.IsNullOrWhiteSpace(rawValues))
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }
    }
}
