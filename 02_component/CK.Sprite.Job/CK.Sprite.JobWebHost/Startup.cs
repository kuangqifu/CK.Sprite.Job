using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using CK.Sprite.Framework;
using CK.Sprite.JobBusiness;
using CK.Sprite.JobCore;

namespace CK.Sprite.JobWebHost
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
            var configuration = services.GetConfiguration();
            services.AddControllersWithViews();

            //添加Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "job Center",
                    Description = "作业管理"
                });
                // 加载程序集的xml描述文档
                var baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                var xmlFile = System.AppDomain.CurrentDomain.FriendlyName + ".xml";
                var xmlPath = Path.Combine(baseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                var jobCoreXmlPath = Path.Combine(AppContext.BaseDirectory, $"CK.Sprite.JobCore.xml");
                c.IncludeXmlComments(jobCoreXmlPath);
            });

            services.AddCors(options => options.AddPolicy("Default",
            builder =>
            {
                builder.WithOrigins(
                            configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
            }));

            services.AddModule(new BusinessModule());
            services.AddModule(new JobCoreModule());
            services.AddModule(new WebHostModule());
            services.StartApp();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSpriteCore();
            app.UseQuartzJob();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "job API V1");
            });
            app.UseCors("Default");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
