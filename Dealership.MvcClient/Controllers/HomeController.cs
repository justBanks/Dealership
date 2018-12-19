using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dealership.MvcClient.ViewModels;

namespace Dealership.MvcClient.Controllers
{
    public class HomeController : Controller
    {
        private static string Message { get; set; } = "";
        private static string Code { get; set; }
        private static string Token { get; set; }
        private static string TokenType { get; set; }
        private static bool LoginAttempted = false;

        private static readonly string State = CryptoRandom.CreateUniqueId();

        public IActionResult Index()
        {
            
            if (!LoginAttempted)
            {
                LoginAttempted = true;
                return Authorize();
            }

            var model = new DefaultViewModel
            {
                DebugMessage = Message,
                IsAuthorized = !string.IsNullOrEmpty(Token)
            };
            return View("Index", model);
        }

        private const string ClientId = "simple_client";
        private const string ClientSecret = "secret";
        private const string RedirectUri = "http://localhost:5001/callback";

        public IActionResult Authorize()
        {
            var authUrl =
                $"http://localhost:5000/connect/authorize?client_id={ClientId}&scope=vehicles_api.read offline_access&redirect_uri={RedirectUri}&response_type=code&response_mode=query&state={State}";

            Message += $"Redirecting to authorization endpoint. State value of: {State}";
            Message += $"\nURL: {authUrl}";
            return Redirect(authUrl);
        }

        public async Task<IActionResult> Callback([FromQuery] string code, [FromQuery] string state)
        {
            if (State != state)
            {
                Message += "\n\nState not recognised. Cannot trust response.";
                return RedirectToAction("Index");
                //return NoContent();
            }

            Code = code;
            await GetToken();

            if(!string.IsNullOrEmpty(Token)) Message += "\n\nApplication Authorized!";
            Message += $"\ncode: {code}";
            Message += $"\nstate: {State}";

            return RedirectToAction("Index");
            //return NoContent();
        }

        public async Task<IActionResult> GetToken()
        {
            if (Code == null)
            {
                Message += "\n\nNot ready! Authorize first.";
                return RedirectToAction("Index");
            }

            Message += "\n\nCalling token endpoint...";
            var tokenClient = new TokenClient("http://localhost:5000/connect/token", ClientId, ClientSecret);
            var tokenResponse = await tokenClient.RequestAuthorizationCodeAsync(Code, RedirectUri);

            if (tokenResponse.IsError)
            {
                Message += "\n\nToken request failed";
                return RedirectToAction("Index");
            }

            TokenType = tokenResponse.TokenType;
            Token = tokenResponse.AccessToken;
            Message += "\n\nToken Received!";
            Message += $"\naccess_token: {tokenResponse.AccessToken}";
            Message += $"\nrefresh_token: {tokenResponse.RefreshToken}";
            Message += $"\nexpires_in: {tokenResponse.ExpiresIn}";
            Message += $"\ntoken_type: {tokenResponse.TokenType}";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CallApi()
        {
            var httpClient = new HttpClient();
            if (Token != null)
            {
                Message += $"\n\nCalling API with Authorization header: {TokenType} {Token}";
                httpClient.SetBearerToken(Token);
            }

            var response = await httpClient.GetAsync("http://localhost:52366/api/vehicles");

            if (response.IsSuccessStatusCode) Message += "\nAPI access authorized!";
            else if (response.StatusCode == HttpStatusCode.Unauthorized) Message += "\nUnable to contact API: Unauthorized!";
            else Message += $"\nUnable to contact API. Status code {response.StatusCode}";

            return RedirectToAction("Index");
        }
    }
}
