using System;

namespace Pattern_of_life.Service
{
    public abstract class AuditableEntity
    {
        public int? TenantId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string? CreatorIp { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModificationTime { get; set; }
        public string? ModifierIp { get; set; }
    }
}
