using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TesteNeonConversaoMoedas.IService;
using TesteNeonConversaoMoedas.Service;

namespace TersteNeonConversaoMoedas
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
            

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddTransient<IConversaoMoedas, ConversaoMoedas>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", p =>
                {
                    p.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddMvc();

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("EnableAjaxAccess", builder =>
            //    {
            //        builder
            //            .AllowAnyOrigin() //TODO: Will be replaced by above when Microsoft releases microsoft.aspnetcore.cors 2.0 - https://github.com/aspnet/CORS/pull/94
            //            .AllowAnyHeader()
            //            .AllowAnyMethod();
            //    });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseCors(options =>
                options.AllowAnyOrigin().AllowAnyMethod()
            );

            app.UseMvc();
        }
    }
}
