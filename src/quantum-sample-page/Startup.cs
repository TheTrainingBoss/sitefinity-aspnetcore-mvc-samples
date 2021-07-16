using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Progress.Sitefinity.AspNetCore;
using Progress.Sitefinity.Clients.LayoutService.Dto;
using Renderer.Models;
using Renderer.Models.Testimonial;

namespace Renderer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // add sitefinity related services
            services.AddScoped<ITestimonialModel, TestimonialModel>();
            services.AddScoped<IMegaMenuModel, MegaMenuModel>();
            services.AddSitefinity();
            services.AddViewComponentModels();
            services.AddMvc().AddViewLocalization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseSitefinity();

            app.Use(async (context, next) =>
            {
                if (context.Items.ContainsKey("sfpagemodel"))
                {
                    var pageNodeDto = context.Items["sfpagemodel"] as PageModelDto;
                    pageNodeDto.UrlParameters = new string[0];
                }

                await next();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapSitefinityEndpoints();
            });
        }
    }
}