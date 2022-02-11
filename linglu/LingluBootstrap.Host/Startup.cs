
using Linglu.Core.AspNetCore;
using Linglu.Core.Middleware; 
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LingluBootstrap
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LingluBootstrap.Host", Version = "v1" });
            });

            services.AddNLogLoggerAspNetCore(Configuration,"nlog.config"); //ע����־����
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRequestResponseLogging();    // ������д���

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LingluBootstrap.Host v1"));
            }


            //�м���ܵ�
            app.UseCloudEvents();

            app.UseSwagger(Configuration);

            

            #region ͳһ�쳣���� �ö�
            app.UseMiddleware<ExceptionMiddleware>();
            #endregion

            app.UseMiddleware<DaprInvokeMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            app.ApplicationServices.GetService<ILogger<Startup>>().LogInformation("����............................");
        }
    }
}
