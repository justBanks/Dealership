using Dealership.API.Entities;
using Dealership.API.Repositories;
using Dealership.MvcClient.ViewModels;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dealership.MvcClient.Controllers
{
    public class HomeController : BaseController
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

            var model = new HomeViewModel
            {
                DebugMessage = Message,
                IsAuthorized = !string.IsNullOrEmpty(Token)
            };
            
            return View("Index", model);
        }

        private string ClientId = Startup.Configuration["ClientCredentials:client_id"];
        private string ClientSecret = Startup.Configuration["ClientCredentials:secret"];
        private const string RedirectUri = "http://localhost:5001/callback";

        public IActionResult Authorize()
        {
            var authUrl =
                $"{authBaseUrl}/authorize?client_id={ClientId}&scope=vehicles_api.read&redirect_uri={RedirectUri}&response_type=code&response_mode=query&state={State}";

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
            var tokenClient = new TokenClient($"{authBaseUrl}/token", ClientId, ClientSecret);
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
            if (Token != null)
            {
                Message += $"\n\nCalling API with Authorization header: {TokenType} {Token}";
                client.SetBearerToken(Token);
            }

            var response = await client.GetAsync($"{apiBaseUrl}/vehicles");

            if (response.IsSuccessStatusCode) Message += "\nAPI access authorized!";
            else if (response.StatusCode == HttpStatusCode.Unauthorized) Message += "\nUnable to contact API: Unauthorized!";
            else Message += $"\nUnable to contact API. Status code {response.StatusCode}";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Search(VehicleSearchModel searchTerms)
        {
            if (!ModelState.IsValid)
                return View(new SearchViewModel { Error = ModelState.Root.Errors[0] });

            client.SetBearerToken(Token);
            HttpResponseMessage response = await 
                client.PostAsJsonAsync(
                    $"{apiBaseUrl}/vehicles", 
                    searchTerms);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var vehicles = JsonConvert.DeserializeObject<List<Vehicle>>(content);

                var model = new SearchViewModel();
                model.Search = searchTerms;
                model.Vehicles = vehicles;
                return View(model);
            }
            else
            {
                return View(
                    "Error",
                    new ErrorViewModel {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        Message = response.ReasonPhrase
                    });
            }
        }
    }

}
