using System.ComponentModel.DataAnnotations.Schema;

namespace Dealership.API.Entities
{
    public partial class Vehicle
    {
        public string Id { get; set; }
        public string Make { get; set; }
        public short Year { get; set; }
        public int Mileage { get; set; }

        [NotMapped]
        public bool HasLowMiles { get => Mileage <= Constants.LOW_MILEAGE_THRESHOLD; }

        public string Color { get; set; }
        public int Price { get; set; }
        public bool HasAutomaticTransmission { get; set; }
        public bool HasSunroof { get; set; }
        public bool IsFourWheelDrive { get; set; }
        public bool HasPowerWindows { get; set; }
        public bool HasNavigation { get; set; }
        public bool HasHeatedSeats { get; set; }
    }
}
