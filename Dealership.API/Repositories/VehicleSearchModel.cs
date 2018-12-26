using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dealership.API.Repositories
{
    public class VehicleSearchModel : IValidatableObject
    {
        private string _color;
        private string _milesLimit;

        public string Color { get => _color; set => _color = string.IsNullOrWhiteSpace(value) ? null : value?.Trim(); }
        public string HasAutomaticTransmission { get; set; }
        public string HasSunroof { get; set; }
        public string IsFourWheelDrive { get; set; }
        public string HasLowMiles { get; set; }
        public string HasPowerWindows { get; set; }
        public string HasNavigation { get; set; }
        public string HasHeatedSeats { get; set; }
        public string milesLimit { get => _milesLimit; set => _milesLimit = string.IsNullOrWhiteSpace(value) ? null : value?.Trim(); }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (milesLimit != null && !float.TryParse(milesLimit, out float f))
                yield return new ValidationResult("Mileage limit must be a number");
        }
    }
}
