using Dealership.API.Entities;
using Dealership.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Dealership.API.Controllers
{
    //ExceptionFilter annotation would be good here, or on a base controller
    [Route("api/vehicles")]
    [Authorize]
    public class VehiclesController : Controller
    {
        private IVehicleRepository _repo;

        public VehiclesController(IVehicleRepository repo) { _repo = repo; }
        //public VehiclesController() { }

        public List<Vehicle> Get()
        {
                return _repo.GetAll();
        }

        [HttpPost]
        public IActionResult Search([FromBody] VehicleSearchModel search)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); //.Root.Errors?[0].ErrorMessage);

            var vehicles = _repo.Query(search);

            if (vehicles.Count == 0)
                return NotFound();

            return Ok(vehicles);
        }
    }
}
