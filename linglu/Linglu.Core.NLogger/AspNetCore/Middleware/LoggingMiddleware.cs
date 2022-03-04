 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection; 
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using System.ComponentModel;
using Linglu.Core.Jwt.Identity;
using Linglu.Core;
using Linglu.Core.Abstractions;

namespace Microsoft.AspNetCore
{ 
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger Logger; 
        private readonly IServiceProvider _provider;
        private   IAttributeValueService _attributeValueService;
        private   ILingluNLogService _lingluNLogService;
        private Stopwatch _stopwatch;
        public LoggingMiddleware(RequestDelegate next,
            IServiceProvider provider,
              ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            Logger = logger;
            _provider = provider;
            _stopwatch = new Stopwatch();
        }
        public async Task Invoke(HttpContext context)
        {
            var controller = (string)context.GetRouteValue("controller");
            var actionName = (string)context.GetRouteValue("action");
            var requestTime = DateTime.Now;
            _stopwatch.Restart();
            var requestText = await FormatRequest(context.Request);

            var originalBodyStream = context.Response.Body;

            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            var responseText = await FormatResponse(context.Response);

            await responseBody.CopyToAsync(originalBodyStream);

            try
            {

                _lingluNLogService = _provider.GetRequiredService<ILingluNLogService>();//lingluNLogService;
                var baseOption = _provider.GetService<LingluLoggerOptions>();
                var agent = context.Request.Headers.FirstOrDefault(u => u.Key.ToUpper().Contains("user-agent".ToUpper()));
                var client = GetAppNameWithVersion(agent.Value);

                var auditLog = GetAuditLoggerData(context,baseOption);
                auditLog.RequestStartTime = requestTime;
                auditLog.ClientModel = client.Key;
                auditLog.AppVer = client.Value;
                auditLog.InParameter = requestText;
                auditLog.OutParameter = responseText;
                auditLog.IpAddress = context.GetClientIP();
                auditLog.RequestEndTime = DateTime.Now;
                auditLog.ControllerDescribe = context.Request.Path;
                auditLog.ControllerName = controller;
                auditLog.ActionName = actionName;
                auditLog.ResultCode = context.Response.StatusCode.ToString();
                auditLog.ResultContent = $"操作成功,耗时:{_stopwatch.ElapsedMilliseconds} ms ";
                _lingluNLogService.LogAudit(NLog.LogLevel.Info, auditLog);
            }
            catch (Exception)
            {

            }
        }


     
        private KeyValuePair<string, string> GetAppNameWithVersion(string agent)
        {
            var res = new KeyValuePair<string, string>();
            var arr = agent.Split(' ');
            if (!arr.Any()) return res;
            if (string.IsNullOrWhiteSpace(arr.FirstOrDefault())) return res;
            var nameValue = arr.FirstOrDefault();
            var nameVersion = nameValue.Split('/');
            if (!nameVersion.Any()) return res;
            var name = nameVersion[0];
            var version = string.Empty;
            if (nameVersion.Length > 1) version = nameVersion[1];
            res = new KeyValuePair<string, string>(name, version);
            return res;
        }

        private LingluAuditLoggerData GetAuditLoggerData(HttpContext httpContext, LingluLoggerOptions opt)
        {
            var data = new LingluAuditLoggerData(opt)
            {
                AccountId = GetPhone(),
            };
            return data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetPhone()
        {
            try
            {
                var identityUserService = _provider.GetRequiredService<IIdentityUserService>();
                var phone = identityUserService.GetUserPhone();
                return phone;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        private static async Task<string> FormatRequest(HttpRequest request)
        { 
            request.EnableBuffering(); 
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length)).ConfigureAwait(false);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin); 
            return $"{bodyAsText}";
        }

        private static async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            if (text.Length > 2000)
            {
                text = text.Substring(0, 2000);
            }
            return text; 
        }
    }

    /// <summary>
    /// 扩展中间件
    /// </summary>
    public static class RequestResponseLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder app)
        {
            app.UseLingluLogger();
            return app.UseMiddleware<LoggingMiddleware>();
        }
    }
}
