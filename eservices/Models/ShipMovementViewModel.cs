using System.ComponentModel.DataAnnotations;
using Pattern_of_life.Models.Entity;

namespace Pattern_of_life.Models
{
    public class ShipMovementViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Vessel Name")]

        public string? VesselName { get; set; }
        public string? IMO { get; set; }
        [Display(Name = "Proposed Arrival")]

        public string? ProposeOfArrival { get; set; }
        public string? Berth { get; set; }
        [Display(Name = "Estimated Arrival Time")]
        public DateTime EAT { get; set; }
        [Display(Name = "Estimated Departure Time")]
        public DateTime ETD { get; set; }
        [Display(Name = "Extra Details")]

        public string? ExtraDetails { get; set; }
        [Required(ErrorMessage = "Please select an Vessel Type")]
        public int VesselTypeID { get; set; }
        public IEnumerable<VesselType>? VesselTypes { get; set; }
        [Required(ErrorMessage = "Please select an Flag States")]
        public int FlagStateID { get; set; }
        public IEnumerable<FlagState>? FlagStates { get; set; }
    }
}
