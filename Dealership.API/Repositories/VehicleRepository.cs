using Dealership.API.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Dealership.API.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        public List<Vehicle> Query(VehicleSearchModel search)
        {
            return
                DataStore.Vehicles
                .Where(v => search.Color == null || v.Color.ToLower() == search.Color.ToLower())
                .Where(v => search.HasAutomaticTransmission == null || v.HasAutomaticTransmission)
                .Where(v => search.HasHeatedSeats == null || v.HasHeatedSeats)
                .Where(v => search.milesLimit == null || v.Mileage <= float.Parse(search.milesLimit))
                .Where(v => search.HasLowMiles == null || (search.milesLimit == null && v.HasLowMiles) || search.milesLimit != null)
                .Where(v => search.HasNavigation == null || v.HasNavigation)
                .Where(v => search.HasPowerWindows == null || v.HasPowerWindows)
                .Where(v => search.HasSunroof == null || v.HasSunroof)
                .Where(v => search.IsFourWheelDrive == null || v.IsFourWheelDrive)
                .ToList();
        }

        public List<Vehicle> GetAll()
        {
            return DataStore.Vehicles;
        }
    }
}
