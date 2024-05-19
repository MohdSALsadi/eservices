using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Pattern_of_life.Services
{
    public class CustomAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly List<string> _requiredRole;

        public CustomAuthorize(string requiredRole)
        {
            _requiredRole = requiredRole.Split(',').ToList();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var sessionData = context.HttpContext.Session.GetString("login-details");

            if (string.IsNullOrEmpty(sessionData))
            {
                context.Result = new RedirectToActionResult("login", "account", null);
                return;
            }

            var loginDetails = JsonConvert.DeserializeObject<UserData>(sessionData);

            if (_requiredRole.Contains("*") && loginDetails.Roles.Any())
            {
                // Allow access for any logged-in user.
            }
            else
            {

                if (loginDetails.Roles.All(e => !_requiredRole.Contains(e)))
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }

}