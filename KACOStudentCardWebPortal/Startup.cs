using System;
using System.Collections.Generic;
using System.Linq;
using BLL;
using BOL.Interfaces;
using DAL;
using DAL.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;

namespace KACOStudentCardWebPortal
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
            services.AddScoped(db =>
            new EntitiesDbContext(Configuration.GetConnectionString("KacoDbConnection")));

            services.AddCors(c =>
            {
                c.AddPolicy("AllowCors", options => options.WithOrigins("http://localhost:4200")
                                       .AllowAnyHeader()
                                       .AllowAnyMethod()
                                       .WithExposedHeaders("x-custom-header")
                                       .SetPreflightMaxAge(TimeSpan.FromSeconds(10000))
                                       );
            });
            //services.AddCors(c =>
            //{
            //    c.AddPolicy("AllowCors", options => options.WithOrigins("http://admin.kwalexculture.org")
            //                               .AllowAnyHeader()
            //                               .AllowAnyMethod()
            //                               .WithExposedHeaders("x-custom-header")
            //                               .SetPreflightMaxAge(TimeSpan.FromSeconds(10000))
            //                               );
            //});
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddTransient<StudentService>();
            services.AddTransient<StudentRepository>();
            services.AddTransient<StudentInfoService>();
            services.AddTransient<StudentInfoRepository>();
            services.AddTransient<ImageService>();
            services.AddTransient<ImageRepository>();
            services.AddTransient<FileService>();
            services.AddTransient<FileRepository>();
            services.AddTransient<CertifiedRecruitmentService>();
            services.AddTransient<CertifiedRecruitmentRepository>();
            services.AddTransient<FeePaymentService>();
            services.AddTransient<FeePaymentRepository>();
            services.AddTransient<SameCollegeSeblingService>();
            services.AddTransient<SameCollegeSeblingRepository>();
            services.AddTransient<CollegeService>();
            services.AddTransient<CollegeRepository>();
            services.AddTransient<UniversityService>();
            services.AddTransient<UniversityRepository>();
            services.AddTransient<ContactService>();
            services.AddTransient<ContactRepository>();
            services.AddTransient<StudentViewService>();
            services.AddTransient<StudentViewRepository>();
            services.AddTransient<ClinicalAllowanceService>();
            services.AddTransient<ClinicalAllowanceRepository>();
            services.AddTransient<TicketExchangeService>();
            services.AddTransient<TicketExchangeRepository>();
            services.AddTransient<AnnualAllowanceExchangeService>();
            services.AddTransient<AnnualAllowanceExchangeRepository>();
            services.AddTransient<SeasonService>();
            services.AddTransient<SeasonRepository>();
            services.AddTransient<UserService>();
            services.AddTransient<UserRepository>();
            services.AddTransient<UserRoleService>();
            services.AddTransient<UserRoleRepository>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseCors("AllowCors");

            app.UseAuthentication();
            app.UseMiddleware(typeof(AuthenticationMiddleware));

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
