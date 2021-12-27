using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4Project.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ConfigurationDbContext _configuration;
        public ClientsController(ConfigurationDbContext configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            var clients = _configuration.Clients.ToList();
            return View(clients);
        }

        public IActionResult Add()
        {
            _configuration.Clients.Add(new IdentityServer4.EntityFramework.Entities.Client
            {
                ClientId = "MeteorologyId",
                ClientSecrets = new List<IdentityServer4.EntityFramework.Entities.ClientSecret>()
                 {
                      new IdentityServer4.EntityFramework.Entities.ClientSecret
                      {
                           Value = "123456".Sha256()
                      }
                 },
                AllowedGrantTypes = new List<IdentityServer4.EntityFramework.Entities.ClientGrantType>()
                 {
                      new IdentityServer4.EntityFramework.Entities.ClientGrantType()
                      {
                           GrantType ="ClientCredentials"
                      }
                 },
                AllowedScopes = new List<IdentityServer4.EntityFramework.Entities.ClientScope>()
                 {
                     new IdentityServer4.EntityFramework.Entities.ClientScope()
                     {
                          Scope="ApiMeteorologyScope"
                     }
                 }

            });
            _configuration.SaveChanges();

            return Ok();
        }
    }
}
