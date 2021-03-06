
using IdentityServer4Project.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace IdentityServer4Project
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
            string cnn = @"Data Source=.;Initial Catalog=IdentityServer4_db;Integrated Security=true;";
            var myAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddControllersWithViews();
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddTestUsers(ConfigurationIdentityServer.GetTestUsers())
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => 
                    b.UseSqlServer(cnn, sql => sql.MigrationsAssembly(myAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => 
                    b.UseSqlServer(cnn, sql => sql.MigrationsAssembly(myAssembly));
                    options.EnableTokenCleanup = true;
                });
            
            //.AddInMemoryApiResources(ConfigurationIdentityServer.GetApiResources())
            //.AddInMemoryIdentityResources(ConfigurationIdentityServer.GetIdentityResources())
            //.AddInMemoryClients(ConfigurationIdentityServer.GetClients())
            //.AddInMemoryApiScopes(ConfigurationIdentityServer.GetApiScopes());
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();
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
