
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

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
            services.AddControllersWithViews();
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddTestUsers(new List<TestUser>()
                {
                    new TestUser()
                    {
                        SubjectId = "1",
                        Username = "ArashM",
                        IsActive = true,
                        Password = "123456",
                        Claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Email,"arash@gmail.com"),
                            new Claim(ClaimTypes.MobilePhone,"011111111"),
                            new Claim("FullName","Arash Mehdipour"),
                        }
                    }
                })
                .AddInMemoryApiResources(new List<ApiResource>()
                {
                })
                .AddInMemoryIdentityResources(new List<IdentityResource>()
                {
                     new IdentityResources.OpenId(),
                     new IdentityResources.Email(),
                     new IdentityResources.Profile(),
                     new IdentityResources.Address(),
                     new IdentityResources.Phone(),
                })
                .AddInMemoryClients(new List<Client>()
                {
                     new Client {
                        ClientId="ClientWebsiteId",
                        ClientSecrets=new List<Secret> { new Secret("123456".Sha256()) },
                        AllowedGrantTypes=GrantTypes.Implicit,
                        RedirectUris={"https://localhost:44365/signin-oidc" },
                        PostLogoutRedirectUris={"https://localhost:44365/"},
                        AllowedScopes =new List<string>
                        {
                            StandardScopes.OpenId,
                            StandardScopes.Profile,
                            StandardScopes.Email,
                            StandardScopes.Phone,
                        },
                        RequireConsent=true,
                     },
                });
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
