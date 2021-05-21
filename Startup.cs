using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FortalezaServer.Models;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FortalezaServer
{
    public class Startup
    {
        public class Dbconfig
        {
            public string Host { get; set; }
            public string Port { get; set; }
            public string User { get; set; }
            public string Password { get; set; }
            public string Database { get; set; }
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    //options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
                    options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                });

            Dbconfig dbconfig = new Dbconfig
            {
                Host = "localhost",
                Port = "3306",
                User = "fortalezait_dbuser",
                Password = "fortalezait@",
                Database = "fortalezaitdb"
            };
            

            string configurationFolderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Fortaleza Server");

            string configurationFilePath = Path.Combine(
                configurationFolderPath,
                "DbConfig.json");

            if(File.Exists(configurationFilePath))
            {
                string jsonString = File.ReadAllText(configurationFilePath);
                dbconfig = JsonConvert.DeserializeObject<Dbconfig>(jsonString);
            }

            services.AddDbContext<fortalezaitdbContext>(options =>
                options.UseMySQL(
                    "server=" + dbconfig.Host +
                    ";port=" + dbconfig.Port +
                    ";user=" + dbconfig.User + 
                    ";password=" + dbconfig.Password +
                    ";database=" + dbconfig.Database )
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
