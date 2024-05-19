using Azure.Core;

namespace eservices.Models
{
    public class RequestRedirectViewModel
    {
        public string DepartmentName { get; set; }
        public string CheckoutByUser { get; set; }
        public DateTime? CheckoutDate { get; set; }
        public string Action { get; set; }
        public string? Decision { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string Status { get; set; }
    }
}
