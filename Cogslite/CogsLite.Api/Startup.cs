using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using GorgleDevs.Mvc;
using CogsLite.MartenStore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace CogsLite.Api
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
            services.AddCors();
            services.AddHttpContextAccessor();
            
            Authentication.LoadKeys();
            services.AddScoped<IUserContext, ClaimsIdentityUserContext>();

            var env = Configuration.GetValue<string>("HOST") ?? "local";
            if(env == "local")
            {
                var dbHost = Configuration.GetValue<string>("DB_HOST") ?? "localhost";
                var dbPort = Configuration.GetValue<string>("DB_PORT") ?? "5432";
                var user = Configuration.GetValue<string>("DB_USER") ?? "postgres";
                var pwd = Configuration.GetValue<string>("DB_PWD") ?? "admin";
                services.AddMarten($"Server={dbHost};Port={dbPort};Database=ccgworks;User Id={user};Password={pwd};");
                services.AddLocalImageStore(@"wwwroot\images\store\", "/images/store/");            
            }
            else
            {
                var awsOptions = Configuration.GetAWSOptions();
                var parameterStore = new ParameterStore(awsOptions);                
                var connectionString = parameterStore.GetParameterAsync("/ccgworks/connection_string").GetAwaiter().GetResult(); 
                services.AddMarten(connectionString);
                services.AddS3ImageStore(awsOptions);
            }
                                    
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKeyResolver = (t, st, kid, v) => Authentication.GetSigningKey(kid),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true
                    };
                });

            services.AddMvc(opts => 
            {
                opts.UseShortGuids();
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.Map(
                    new PathString("/health-check"),
                    a => a.Use(async (context, next) =>
                    {
                        await context.Response.WriteAsync("ok");
                    }));
            }

            app.Map(new PathString("/info"),
                a => a.Use(async (context, next) => 
                {
                    await context.Response.WriteAsync("ccgworks:api-0.1");
                }));
            
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseMvc();
        }                
    }
}
