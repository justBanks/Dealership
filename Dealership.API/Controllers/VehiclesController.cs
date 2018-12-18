using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Dealership.API.Controllers
{
    [Route("api/vehicles")]
    [Authorize]
    public class VehiclesController : Controller
    {
        public IActionResult Get()
        {
            return Content("API says hello");
        }
    }
}
