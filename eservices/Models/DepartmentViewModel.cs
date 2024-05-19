using System.ComponentModel.DataAnnotations;

namespace eservices.Models
{
    public class DepartmentViewModel
    {
        public int Id { get; set; } 
        [Required]
        public required string DepartmentName { get; set; }
        public string Shortcut { get; set; }
        public int? ParentId { get; set; }
    }
}
