using System.ComponentModel.DataAnnotations;
using Pattern_of_life.Models.Entity;

namespace Pattern_of_life.Models
{
    public class ShipActivityViewModel
    {
        public int ID { get; set; }
        public DateTime DTG { get; set; }
        public double Longitude { get; set; } 
        public double Latitude { get; set; }  
        public string? LatitudeDMS { get; set; }
        public string? LongitudeDMS { get; set; }
        public string? ImagePath { get; set; }

        public double Course { get; set; }
        public int IMO { get; set; }
        public string? POB { get; set; } 
        public string? Remarks { get; set; } 
        public double Speed { get; set; }
        public string? Name { get; set; }
        public string? SideNumber { get; set; }

        [Required(ErrorMessage = "Please select an Vessel Type")]
        public int VesselTypeID { get; set; }
        public IEnumerable<VesselType>? VesselTypes { get; set; }
        [Required(ErrorMessage = "Please select an Flag States")]
        public int FlagStateID { get; set; }
        public IEnumerable<FlagState>? FlagStates { get; set; }

        [Required(ErrorMessage = "Please select an Activity Name")]
        public int ActivityNameID { get; set; }
        public IEnumerable<ActivityName>? ActivityNames { get; set; }


    }
}