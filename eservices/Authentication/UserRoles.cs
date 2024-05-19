using Microsoft.AspNetCore.Identity;

namespace Pattern_of_life
{
    internal class UserRoles : IdentityUserRole<string>
    {
        public string UserId { get; set; }
        public int RoleId { get; set; }
    }
}