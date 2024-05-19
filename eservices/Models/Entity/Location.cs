using System.ComponentModel.DataAnnotations;

namespace Pattern_of_life.Models.Entity
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
