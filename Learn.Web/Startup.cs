using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Learn.Core.Convertors;
using Learn.Core.Services;
using Learn.Core.Services.Interfaces;
using Learn.DataLayer.Context;
using Learn.Core.Repository.Interfaces;
using Learn.Core.Repository.Services;

namespace Learn.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            //MVC
            services.AddMvc(MvcOptions => MvcOptions.EnableEndpointRouting = false);
            services.AddRazorPages();

            //Baraye Upload kardan file ba hajme balla dar system amel haye Dgari be joz windows
            //services.Configure<FormOptions>(options => { options.MultipartBodyLengthLimit = 6000000; });

            #region Authentication

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(option =>
            {
                option.LoginPath = "/Login";
                option.LogoutPath = "/LogOut";
                option.ExpireTimeSpan = TimeSpan.FromMinutes(43200);
            });

            #endregion
         

            #region DataBaseContext

            services.AddDbContext<LearnContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("LearnConnection"));
            });

            #endregion

            #region IoC


            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IPagesService, PagesService>();

            services.AddScoped<IViewRenderService, RenderViewToString>();




            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            //app.UseHttpsRedirection();
            //app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}"
              );

                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });
            //app.UseEndpoints(endpoints =>
            //{

            //    endpoints.MapRazorPages();
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //    endpoints.MapAreaControllerRoute(name: "areas",
            //        areaName: "userpanel",
            //        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


            //});


        }
    }
}
