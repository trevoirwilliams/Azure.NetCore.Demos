using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace ClassifiedsAzureADAuth.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ClassifiedsListingController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Shoes", "Cars", "Jackets", "Houses", "Apartments", "Gear", "Sports", "Technology", "Phones", "Laptops"
        };

        private readonly ILogger<ClassifiedsListingController> _logger;
        private readonly IHttpClientFactory httpClientFactory;

        public ClassifiedsListingController(ILogger<ClassifiedsListingController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IEnumerable<ClassifiedsListing>> GetAsync()
        {
            var currentToken = await HttpContext.GetTokenAsync("access_token");
            var httpClient = httpClientFactory.CreateClient();
            var tokenEndpointResponse = await httpClient.RequestTokenAsync(
                new TokenRequest 
                { 
                    Address = "https://login.microsoftonline.com/4e3149fe-4596-4f1d-858a-882973ab5062/oauth2/v2.0/token",
                    GrantType = "urn:ietf:params:oauth:grant-type:jwt-bearer",
                    ClientId = "a9d87f9d-bc8a-4c5b-8af3-87292c4872b9",
                    ClientSecret = "0ps7Q~DJfbKkDA5cRK1NSapz3e8TlrOuzAemm",
                    Parameters =
                    {
                        {"assertion",currentToken},
                        {"scope","api://b6d35d14-91e0-4e05-afa7-a8919b56011b/AllAccess"},
                        {"requested_token_use","on_behalf_of"},
                    }
                });

            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44313/weatherforecast");
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, tokenEndpointResponse.AccessToken);

            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(response.StatusCode.ToString());
            }

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new ClassifiedsListing
            {
                Date = DateTime.Now.AddDays(index),
                Sold = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
