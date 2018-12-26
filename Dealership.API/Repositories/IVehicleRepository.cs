using Dealership.API.Entities;
using System.Collections.Generic;

namespace Dealership.API.Repositories
{
    public interface IVehicleRepository
    {
        List<Vehicle> Query(VehicleSearchModel search);
        List<Vehicle> GetAll();
    }
}
