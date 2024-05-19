using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eservices.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(20)]
        public string ContactNumber { get; set; }

        [Required]
        public string Password { get; set; }

        public int? DepartmentId { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public bool IsActive { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
    }
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
    public class UserRole
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
    public class Request
    {
        [Key]
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Originator { get; set; }
        public string ReferenceNumber { get; set; }
        public int EstablishmentId { get; set; }
        public string Applicability { get; set; }
        public string? Description { get; set; }
        public string? TaskType { get; set; }
        public string? Effect { get; set; }
        public string? Effect2 { get; set; }
        public string? AttachmentUrl { get; set; }
        public string Status { get; set; }
        public string? Comment { get; set; }
        public string? RejectionNotes { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int CreatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;

        [ForeignKey("CreatedBy")]
        public virtual User CreatedUser { get; set; }
        public DateTime? UpdateOn { get; set; }
        public int? UpdatedBy { get; set; }
        [ForeignKey("UpdatedBy")]
        public virtual User UpdatedUser { get; set; }

        [ForeignKey("EstablishmentId")]
        public virtual Establishment Establishment { get; set; }



    }
   
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Shortcut { get; set; }
        public int? ParentId { get; set; }
        public virtual List<DepartmentDecision> DepartmentDecisions { get; set; }
        [ForeignKey("ParentId")]
        public virtual Department Parent { get; set; }
    }
    
    public class Establishment
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Shortcut { get; set; }
    }
    public class DecisionType
    {
       public int Id { get; set; }
      public required string TypeName { get; set; }
    }
    public class DepartmentDecision
    {
        public int Id { get; set; }
        public required int DepartmentId { get; set; }
        public int DecisionTypeId { get; set; }
        public string? Label { get; set; }
        public string? PlaceHolder { get; set; }
        public string? Options { get; set; }
        public string? Group { get; set; }
        public string? Alias { get; set; }

        [ForeignKey("DepartmentId")]
        public  virtual Department Department { get; set; }

        [ForeignKey("DecisionTypeId")]
        public  virtual DecisionType DecisionType { get; set; }

    }
    public class RequestRedirect
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int DepartmentId { get; set; }
        public int? CheckoutByUserId { get; set; }
        public DateTime? CheckoutDate { get; set; }
        public string Status { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public int CreatedById { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("UpdatedBy")]
        public virtual User UpdatedByUser { get; set; }


        [ForeignKey("CheckoutByUserId")]
        public virtual User CheckoutByUser { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

    }

    public class RequestRedirectResult
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int DepartmentDecisionId { get; set; }
        public string? DataValue { get; set; }

        [ForeignKey("DepartmentDecisionId")]
        public virtual DepartmentDecision DepartmentDecision { get; set; }

        [ForeignKey("RequestId")]
        public virtual Request Request { get; set; }
    }

    public class CoreNotification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }
        [ForeignKey("UpdatedBy")]
        public virtual User UpdatedByUser { get; set; }
    }

}
