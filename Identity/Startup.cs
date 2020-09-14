using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Data;
using Identity.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Identity
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
             //.AddClaimsPrincipalFactory<MyUserClaimsPrincipalFactory>()             
             .AddDefaultTokenProviders()
             .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                option.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                option.DefaultSignInScheme = IdentityConstants.ExternalScheme;

            });


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)

                .AddCookie(option =>
                {
                    option.Cookie.SameSite = SameSiteMode.Lax;
                    option.Cookie.HttpOnly = true;
                    option.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    option.SlidingExpiration = true;
                    option.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                });



            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
