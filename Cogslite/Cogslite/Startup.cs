using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CogsLite.Core;
using CogsLite.MartenStore;
using GorgleDevs.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;

namespace Cogslite
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
            services.AddMvc().AddRazorPagesOptions(opt =>
            {
                opt.Conventions.AddPageRoute("/Home", "");
            });

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/SignIn";
            });

            services.AddMarten("Server=localhost;Port=5432;Database=Cogs;User Id=postgres;Password=admin;");

			services.AddMvc(opts =>
			{
				opts.UseShortGuids();
			})            
			.AddRazorOptions(options =>
			{
				options.PageViewLocationFormats.Add("/Pages/Dialogs/{0}.cshtml");				
			});                    			
        }        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Map(
                new PathString("/health-check"),
                a => a.Use(async (context, next) =>
                {
                    await context.Response.WriteAsync("ok");
                }));

            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
