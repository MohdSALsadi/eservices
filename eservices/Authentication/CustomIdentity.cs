using System.Security.Claims;

namespace eservices.Authentication
{
    public class CustomIdentity : ClaimsIdentity
    {
        public CustomIdentity(ClaimsIdentity identity) : base(identity)
        {
        }

        public long? UserId
        {
            get
            {
                var userIdClaim = FindFirst("userId");
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return userId;
                }
                return null;
            }
        }

        public int? DepartmentId
        {
            get
            {
                var departmentIdClaim = FindFirst("departmentId");
                if (departmentIdClaim != null && int.TryParse(departmentIdClaim.Value, out int departmentId))
                {
                    return departmentId;
                }
                return null;
            }
        }
    }
}
