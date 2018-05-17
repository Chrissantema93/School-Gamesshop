using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Explordamweb.Models;
using Explordamweb.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Explordamweb.Services;

namespace Explordamweb
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;
        public IConfiguration Configuration { get; }



        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseNpgsql(
                    Configuration["Data:Explordamweb:ConnectionString"]));

            services.AddDbContext<AppIdentityDbContext>(options =>
                        options.UseNpgsql(
                        Configuration["Data:Explordamweb:ConnectionString"]));


            services.AddIdentity<AppUser, IdentityRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;
                opts.SignIn.RequireConfirmedEmail = true;
            })

            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();

            //hiermee kan je de criteria voor passwords veranderen, spreekt voor zich
//            services.AddIdentity<AppUser, IdentityRole>(opts => {
//                opts.Password.RequiredLength = 6;
//                opts.Password.RequireNonAlphanumeric = false;
//                opts.Password.RequireLowercase = false;
//                opts.Password.RequireUppercase = false;
//                opts.Password.RequireDigit = false;
//            }).AddEntityFrameworkStores<AppIdentityDbContext>()
//.AddDefaultTokenProviders();



            services.AddTransient<IGamesRepository, EFGamesRepository>();
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IOrderRepository, EFOrderRepository>();
            services.AddTransient<IUserRepository, EFUserRepository>();
            services.AddTransient<IWishListRepository, EFWishListRepository>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddMvc();
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.AddMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseMvc(routes => {
                routes.MapRoute(
                        name: null,
                        template: "",
                        defaults: new { controller = "Home", action = "Home" });
                routes.MapRoute(
                        name: null,
                        template: "Admin/DeleteUser/{name}",
                        defaults: new { controller = "Admin", action = "DeleteUser" });
                routes.MapRoute(
                        name: null,
                        template: "Admin/List/{sorton}/Page{GamesPage:int}",
                        defaults: new { controller = "Admin", action = "List" });
                routes.MapRoute(
                        name: null,
                        template: "Games/SearchList/{searchstring}",
                        defaults: new { controller = "Games", action = "SearchList" });
                routes.MapRoute(
                        name: null,
                        template: "{Games}/{Genre}/{Filter}/{platform}/Page{GamesPage:int}",
                        defaults: new { controller = "Games", action = "AltList" });
                routes.MapRoute(
                        name: null,
                        template: "{Genre}/Page{GamesPage:int}",
                        defaults: new { controller = "Games", action = "List" });
                routes.MapRoute(
                        name: null,
                        template: "{genre}/{SortOn}/{ascordesc}/Page{GamesPage:int}",
                        defaults: new { controller = "Games", action = "SortList" });
                routes.MapRoute(
                        name: null,
                        template: "{Games}/{Genre}/{Name}",
                        defaults: new { controller = "Games", action = "GameInfo" });
                routes.MapRoute(name: null,
                        template: "{controller}/{action}/{ID?}",
                        defaults: null);
                routes.MapRoute(name: null,
                        template: "{controller}/{action}/{name?}",
                        defaults: null);
                routes.MapRoute(name: null,
                        template: "{controller}/{action}",
                        defaults: null);
                

            });

        }

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
    }
}

