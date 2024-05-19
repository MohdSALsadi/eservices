namespace Pattern_of_life.Models.Entity
{
    public class ShipMovement
    {
        public int Id { get; set; }
        public string? VesselName { get; set; }
        public string? IMO { get; set; }
        public string? ProposeOfArrival { get; set; }
        public string? Berth { get; set; }
        public DateTime EAT { get; set; }
        public DateTime ETD { get; set; }
        public string? ExtraDetails { get; set; }
        // Foreign keys
        public int VesselTypeID { get; set; }
        public int FlagStateID { get; set; }

        // Navigation properties
        public VesselType? VesselType { get; set; }
        public FlagState? FlagState { get; set; }
    }
}
