using System.ComponentModel.DataAnnotations;

namespace eservices.Models
{
    public class EstablishmentViewModel
    {
        public int Id { get; set; } 

        [Required]
        public required string EstablishmentName { get; set; }
        public string Shortcut { get; set; }
    }
}
