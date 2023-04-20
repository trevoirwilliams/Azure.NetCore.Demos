using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ClassifiedsAzureADAuth.Client.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration _configuration;

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _logger = logger; 
            this.httpClientFactory = httpClientFactory;
            this._configuration = configuration;
        }

        public async Task OnGet()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            ////var httpClient = httpClientFactory.CreateClient();

            ////var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44336/classifiedslisting");
            ////request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

            ////var response = await httpClient.SendAsync(request);

            ////if (!response.IsSuccessStatusCode)
            ////{
            ////    _logger.LogError(response.StatusCode.ToString());
            ////}
        }
    }
}
