using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using PSTS6.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using PSTS6.Areas.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using PSTS6.Configuration;
using PSTS6.Repository;

namespace PSTS6
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
            
            
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddAutoMapper(typeof(Startup));

            services.AddDbContext<PSTS6Context>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("PSTS6Context")));
            services.AddScoped<IRepository, DbRepository>();


            #region IdentityConfiguration
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<PSTS6Context>();
            #endregion

            #region AuthorizationConfiguration
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();

                options.AddPolicy("EditDetailsActivityPolicy",
                    policy => policy.AddRequirements(new ManageEditDetailsActivityRequirements()))
                ;
                options.AddPolicy("EditDetailsProjectPolicy",
                    policy => policy.AddRequirements(new ManageEditDetailsProjectRequirements()));

                options.AddPolicy("CreateDeleteActivitiesPolicy",
                    policy => policy.AddRequirements(new ManageCreateDeleteActivityRequirements()));

                options.AddPolicy("CreateDeleteProjectPolicy",
                    policy => policy.AddRequirements(new ManageCreateDeleteProjectRequirements()));
                options.AddPolicy("CreateDeleteTaskPolicy",
                    policy => policy.AddRequirements(new ManageCreateDeleteTaskRequirements()));
                options.AddPolicy("EditDetailsTaskPolicy",
                    policy => policy.AddRequirements(new ManageEditDetailsTaskRequirements()));
            });

            services.AddTransient<IAuthorizationHandler, CanOnlyEditOwnedActivitiesHandler>();
            services.AddTransient<IAuthorizationHandler, CanOnlyEditViewDeleteProjectsWhereIsPMOrAdmin>();
            services.AddTransient<IAuthorizationHandler, CanOnlyCreateDeleteActivitiesFromOwnProject>();
            services.AddTransient<IAuthorizationHandler, CanOnlyEditViewTasksFromOwnProjects>();
            services.AddTransient<IAuthorizationHandler, CanOnlyCreateDeleteOwnProjects>();
            services.AddTransient<IAuthorizationHandler, CanOnlyCreateDeleteTasksFromOwnProjects>();
            #endregion


            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<PSTS6.HelperClasses.BackgroundCalculations>();

            #region SettingsConfiguration


            services.Configure<ProjectSettings>(Configuration.GetSection("ProjectCreationMode"));
            services.Configure<DefaultDateModeSettings>(Configuration.GetSection("DefaultDateMode"));
            services.Configure<ActualEndDateModeSettings>(Configuration.GetSection("ActualEndDateMode"));

            #endregion



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
