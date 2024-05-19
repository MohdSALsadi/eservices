using eservices.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;
using Pattern_of_life;

namespace eservices.Controllers
{
    [CustomAuthorize("*")]
    public class BaseController : Controller
    {
        public int? UserId
        {
            get
            {
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return null;
                else return int.Parse(userIdClaim.Value);
            }
        }

        public int ? DepartmentId
        {
            get
            {
                var departmentIdClaim= HttpContext.User.FindFirst("DepartmentId");
                if (departmentIdClaim == null)
                    return null;
                else return int.Parse(departmentIdClaim.Value);
            }
        }

        public string? DepartmentName
        {
            get
            {
                var departmentNameClaim = HttpContext.User.FindFirst("DepartmentName");
                if (departmentNameClaim == null)
                    return null;
                else return departmentNameClaim.Value;
            }
        }

        public string? Username => User?.Identity?.Name;
        public string Language => CultureInfo.CurrentCulture.Name.StartsWith("ar", StringComparison.OrdinalIgnoreCase) ? "Arabic" : "English";
        public List<string> Roles
        {
            get
            {
                var roles = new List<string>();

                var user = HttpContext.User;

                if (user.Identity.IsAuthenticated)
                {
                    roles.AddRange(user.FindAll(ClaimTypes.Role).Select(c => c.Value));
                }

                return roles;
            }
        }

        public override ViewResult View()
        {
            ViewBag.Language = Language;
            ViewBag.Roles = Roles;
            return base.View();
        }

        public override ViewResult View(object model)
        {
            ViewBag.Language = Language;
            ViewBag.Roles = Roles;
            return base.View(model);
        }

        public override ViewResult View(string viewName)
        {
            ViewBag.Language = Language;
            ViewBag.Roles = Roles;
            return base.View(viewName);
        }

        public override ViewResult View(string viewName, object model)
        {
            ViewBag.Language = Language;
            ViewBag.Roles = Roles;
            return base.View(viewName, model);
        }
    }
}
