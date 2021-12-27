using IdentityModel.Client;
using System;
using System.Net.Http;

namespace ClientConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            HttpClient client = new HttpClient();
          
            var discoveryDocument = client.GetDiscoveryDocumentAsync("https://localhost:5001").Result;

            if (discoveryDocument.IsError)
            {
                Console.WriteLine(discoveryDocument.Error);
                return;
            }


            var accessToken = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = "MeteorologyId",
                ClientSecret = "123456",
                Scope = "ApiMeteorologyScope"
            }).Result;

            if (accessToken.IsError)
            {
                Console.WriteLine(discoveryDocument.Error);
                return;
            }

            Console.WriteLine(accessToken.Json);


            HttpClient apiClient = new HttpClient();
            apiClient.SetBearerToken(accessToken.AccessToken);
            var result = apiClient.GetAsync("https://localhost:44341/WeatherForecast").Result;
            var content = result.Content.ReadAsStringAsync().Result;
            Console.WriteLine(content);


            Console.WriteLine("Hello World!");
        }
    }
}
