using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RouteSample.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteSample
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
            services.Configure<RouteOptions>(opt => opt.ConstraintMap.Add("custom", typeof(CustomConstraint)));
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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

            app.UseEndpoints(endpoints =>
            {
                //Default deðerleri barýndýrýr. 
                endpoints.MapDefaultControllerRoute(); 

                //Custom Route
                endpoints.MapControllerRoute("Default2", "anasayfa", new { controller = "Home", action = "Index" });
                endpoints.MapControllerRoute("Default", "{controller=Personel}/{action=Index}");


                //route constraints 
                endpoints.MapControllerRoute("length", "{controller=home}/{action=Index}/{id:int?}/{x:length(12)?}");
                endpoints.MapControllerRoute("maxlength", "{controller=Personel}/{action=Index}/{id:maxlength(10)}");
                endpoints.MapControllerRoute("minlength", "{controller=Personel}/{action=Index}/{id:minlength(10)}");
                endpoints.MapControllerRoute("range", "{controller=Personel}/{action=Index}/{id:range(5,10)}");
                endpoints.MapControllerRoute("min", "{controller=Personel}/{action=Index}/{id:min(5)}");
                endpoints.MapControllerRoute("max", "{controller=Personel}/{action=Index}/{id:max(10)}");
                endpoints.MapControllerRoute("Default4", "{controller=Personel}/{action=Index}/{id:alpha:maxlength(10)}");

                //Route Özelden genele göre sýralanmalýdýr. 
            });
        }
    }
}
