using GeeksForLessMVC.Contracts;
using GeeksForLessMVC.Data;
using GeeksForLessMVC.Interfaces;
using GeeksForLessMVC.Service;
using Microsoft.EntityFrameworkCore;

namespace GeeksForLessMVC
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ITreeService, TreeService>();
            builder.Services.AddScoped(typeof(ITreeData), typeof(TreeData));

            string connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(connection)); //SQL

            builder.Services.AddMemoryCache();

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            await SeedDatabaseAsync(app);

            app.Run();
        }

        private static async Task SeedDatabaseAsync(WebApplication host)
        {
            using (var scope = host.Services.CreateScope())
            {
                IServiceProvider scopeServiceProvider = scope.ServiceProvider;

                var context = scopeServiceProvider.GetRequiredService<MyDbContext>();

                if (context.Database.EnsureCreated())
                {
                }

            }
        }
    }
}