namespace Pattern_of_life.Models.Entity
{
    public class VesselType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<ShipActivity>? ShipActivities { get; set; }
    }
}
