using System;
using Company.G04.BLL.Interfaces;
using Company.G04.BLL.Repositries;
using Company.G04.BLL.UnitOfWork;
using Company.G04.DAL.Data.Context;
using Company.G04.DAL.Moudel;
using Company.G04.PL.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Company.G04.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository>(); // Allow DI For DepartmentRepository
            builder.Services.AddScoped<IEmployeeRepository,EmployeeRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddDbContext<CompanyDbContext>(options=>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaltConnection"));
            }); // Allow DI For CompanyDbContext

            builder.Services.AddAutoMapper(M => M.AddProfile(new MapProfile()));

            builder.Services.AddAutoMapper(E => E.AddProfile(new MapProfile()));
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<CompanyDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
            });
 
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
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
         

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
