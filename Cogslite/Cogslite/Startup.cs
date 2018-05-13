using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CogsLite.MongoStore;
using CogsLite.Core;
using GorgleDevs.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

			services.AddMvc(opts =>
			{
				opts.UseShortGuids();
			})
			.AddRazorOptions(options =>
			{
				options.PageViewLocationFormats.Add("/Pages/Dialogs/{0}.cshtml");				
			});

			services.AddTransient<IUserStore, UserStore>();
            services.AddTransient<IGameStore, GameStore>();
            services.AddTransient<IImageStore, ImageStore>();
            services.AddTransient<ICardStore, CardStore>();
            services.AddTransient<IDeckStore, DeckStore>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
