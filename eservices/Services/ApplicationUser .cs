using Microsoft.AspNetCore.Identity;
using Pattern_of_life.Service;

namespace Pattern_of_life.Services
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int? LocationId { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
    }
}
//int userLocation = userManager.FindByIdAsync(contextAccessor.UserId()).Result.LocationId ?? 0;
//if (!model.IsSubmit)
//{
//    model.IsSubmit = false;
//    model.SubmitBy = this.contextAccessor.UserName();
//    model.SubmitDate = DateTime.Now;
//    model.DemandModifiedBy = contextAccessor.UserId();
//    model.DemandModifiedIp = contextAccessor.IPAddress();
//    model.Status = 0;
//}


//bool isAdmin = IsAdminUser();
//private bool IsAdminUser()
//{
//    return userManager.FindByIdAsync(contextAccessor.UserId()).Result.IsAdmin;
//}
