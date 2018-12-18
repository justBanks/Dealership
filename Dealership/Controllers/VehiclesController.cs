using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;

namespace Dealership.Controllers
{
    public class VehiclesController : Controller
    {
        private static string Message { get; set; } = "";
        private static string Code { get; set; }
        private static string Token { get; set; }

        [Route("vehicles")]
        public async Task<IActionResult> Index()
        {
            return Content(Message);
        }

        private const string ClientId = "clientcreds_client";
        private const string ClientSecret = "secret";
        private const string RedirectUri = "http://localhost:50375/callback";

        public IActionResult Authorize()
        {
            Message += "\n\nRedirecting to authorization endpoint...";
            return Redirect($"http://localhost:5000/connect/authorize?client_id={ClientId}&scope=vehicles_api.read&redirect_uri={RedirectUri}&response_type=token&response_mode=query");
        }

        [Route("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code)
        {
            Code = code;
            Message += "\nApplication Authorized!";

            Message += "\n\nCalling token endpoint...";
            var tokenClient = new TokenClient("http://localhost:5000/connect/token", ClientId, ClientSecret);
            var tokenResponse = await tokenClient.RequestAuthorizationCodeAsync(Code, RedirectUri);

            if (tokenResponse.IsError)
            {
                Message += "\nToken request failed";
                //return RedirectToAction("Index");
                return RedirectToRoute("vehicles");
            }

            Token = tokenResponse.AccessToken;
            Message += "\nToken Received!";

            return RedirectToAction("Index");
        }

        [Route("callapi")]
        public async Task<IActionResult> CallApi()
        {
            var httpClient = new HttpClient();
            if (Token != null) httpClient.SetBearerToken(Token);

            var response = await httpClient.GetAsync("http://localhost:52366/api/vehicles");

            if (response.IsSuccessStatusCode) Message += "\n\nAPI access authorized!";
            else if (response.StatusCode == HttpStatusCode.Unauthorized) Message += "\nUnable to contact API: Unauthorized!";
            else Message += $"\n\nUnable to contact API. Status code {response.StatusCode}";

            return RedirectToAction("Index");
        }
    }
}