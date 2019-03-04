using Dealership.API.Controllers;
using Dealership.API.Entities;
using Dealership.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Net.Http;

namespace Tests
{
    [TestClass]
    public class TheVehiclesController
    {
        private Mock<IVehicleRepository> mock = new Mock<IVehicleRepository>();
        private VehiclesController controller;

        [TestInitialize]
        public void Setup()
        {
            controller = new VehiclesController(mock.Object);       
        }

        [TestMethod]
        public void Returns400ForInvalidSearchParams()
        {
            var searchParams = new VehicleSearchModel { milesLimit = "word" };

            controller.ModelState.AddModelError("ModelError", "Mileage limit must be a number");
            var result = controller.Search(searchParams);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void Returns404ForEmptySearchResults()
        {
            mock.Setup(x => x.Query(It.IsAny<VehicleSearchModel>())).Returns(new List<Vehicle>());
            var searchParams = new VehicleSearchModel { Color = "Black", HasHeatedSeats = "yes" };

            var result = controller.Search(searchParams);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void ReturnsVehiclesFromTheRepository()
        {
            mock.Setup(x => x.GetAll()).Returns(new List<Vehicle> {
                new Vehicle{ Color = "Silver", Make = "Mercedes", Mileage = 400 }
            });
            var result = controller.Get();

            Assert.IsTrue(result.Count > 0);
        }
    }
}
