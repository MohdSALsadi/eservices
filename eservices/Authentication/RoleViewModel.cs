using System.ComponentModel.DataAnnotations;

namespace eservices.Authentication
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        public string? Name { get; set; }

        public string? NormalizedName { get; set; }

    }
}
