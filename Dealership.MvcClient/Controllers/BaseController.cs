using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace Dealership.MvcClient.Controllers
{
    public class BaseController : Controller
    {
        protected readonly HttpClient client = new HttpClient();
        protected readonly string apiBaseUrl = "http://localhost:52366/api";
        protected readonly string authBaseUrl = "http://localhost:5000/connect";
    }
}
