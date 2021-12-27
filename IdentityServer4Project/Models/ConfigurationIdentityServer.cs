using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using static IdentityServer4.IdentityServerConstants;
namespace IdentityServer4Project.Models
{
    public static class ConfigurationIdentityServer
    {
        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>()
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
            };
        }
        public static List<Client> GetClients()
        {
            return
                new List<Client>()
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
                     new Client()
                     {
                         ClientId = "MeteorologyId",
                         ClientSecrets=new List<Secret> { new Secret("123456".Sha256()) },
                         AllowedGrantTypes = GrantTypes.ClientCredentials,
                         AllowedScopes = new []{ "ApiMeteorologyScope" }
                     }
                };
        }
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
                {
                     new IdentityResources.OpenId(),
                     new IdentityResources.Email(),
                     new IdentityResources.Profile(),
                     new IdentityResources.Address(),
                     new IdentityResources.Phone(),
                };
        }
        public static List<ApiResource> GetApiResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("ApiMeteorology","Meteorology Services")
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>()
            {
                new ApiScope("ApiMeteorologyScope","Meteorology Services")
            };
        }
    }
}
