using Dealership.API.Controllers;
using Dealership.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;

namespace Tests
{
    [TestClass]
    public class TheVehiclesController
    {
        [TestMethod]
        public void Returns400ForInvalidSearchParams()
        {
            var controller = new VehiclesController();
            var searchParams = new VehicleSearchModel { milesLimit = "word" };

            controller.ModelState.AddModelError("ModelError", "Mileage limit must be a number");
            var result = controller.Search(searchParams);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
        [TestMethod]
        public void Returns404WhenNoSearchResultsFound()
        {
            var controller = new VehiclesController();
            var searchParams = new VehicleSearchModel { Color = "Black", HasHeatedSeats = "yes" };

            var result = controller.Search(searchParams);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void ReturnsVehiclesFromTheRepository()
        {
            var controller = new VehiclesController();

            var result = controller.Get();

            Assert.IsTrue(result.Count > 0);
        }
    }
}
