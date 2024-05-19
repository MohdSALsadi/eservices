using System.ComponentModel.DataAnnotations;

namespace Pattern_of_life.Models.Entity
{
    public class FlagState
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [Display(Name = "Flag Image")]
        public string? FlagImagePath { get; set; }

        public ICollection<ShipActivity>? ShipActivities { get; set; }
    }
}
