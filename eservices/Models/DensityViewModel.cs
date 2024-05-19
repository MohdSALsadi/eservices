using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Pattern_of_life.Models.Entity;

namespace Pattern_of_life.Models
{
    public class DensityViewModel
    {
        public IEnumerable<ShipActivity>? ShipActivities { get; set; }
        public Dictionary<Tuple<double, double>, double>? DensityMap { get; set; }
    }
}
