using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Controllers;
using WebApplication4.Services;

namespace WebApplication4
{
    public class Startup
    {
        public ParserService _parserservice;
        public ParserController _parsercontroller;
        public LoaderService _loaderservice;
        public LoaderController _loadercontrolller;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //_parserservice = new ParserService(configuration);
            //_parsercontroller = new ParserController(_parserservice);
            //_loaderservice = new LoaderService(configuration);
            //_loadercontrolller = new LoaderController(_loaderservice);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApplication4", Version = "v1" });
            });

            // Configure Services
            services.AddTransient<LoaderService>();
            services.AddTransient<ParserService>();
            services.AddTransient<AggregatorService>();
            services.AddTransient<UiService>();
            services.AddTransient<DailyUiService>();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication4 v1"));

            }
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //FileSystemWatcher watcher = new FileSystemWatcher(@"C:\Users\User\Desktop\InFolder");

            //watcher.NotifyFilter = NotifyFilters.Attributes
            //                     | NotifyFilters.CreationTime
            //                     | NotifyFilters.DirectoryName
            //                     | NotifyFilters.FileName
            //                     | NotifyFilters.LastAccess
            //                     | NotifyFilters.LastWrite
            //                     | NotifyFilters.Security
            //                     | NotifyFilters.Size;

            //watcher.Changed += ParserController.OnChanged;
            //watcher.Filter = "*.txt";
            //watcher.IncludeSubdirectories = true;
            //watcher.EnableRaisingEvents = true;

            //FileSystemWatcher watcherLoader = new FileSystemWatcher(@"C:\Users\User\Desktop\ParsedData");

            //watcher.NotifyFilter = NotifyFilters.Attributes
            //                     | NotifyFilters.CreationTime
            //                     | NotifyFilters.DirectoryName
            //                     | NotifyFilters.FileName
            //                     | NotifyFilters.LastAccess
            //                     | NotifyFilters.LastWrite
            //                     | NotifyFilters.Security
            //                     | NotifyFilters.Size;

            //watcher.Changed += LoaderController.OnCreated;
            //watcher.Filter = "*.txt";
            //watcher.IncludeSubdirectories = true;
            //watcher.EnableRaisingEvents = true;


        }







    }
}
