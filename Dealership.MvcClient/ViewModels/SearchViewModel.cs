using Dealership.API.Entities;
using Dealership.API.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Dealership.MvcClient.ViewModels
{
    public class SearchViewModel
    {
        public VehicleSearchModel Search;
        public IEnumerable<Vehicle> Vehicles = new List<Vehicle>();
        public ModelError Error;
    }
}
