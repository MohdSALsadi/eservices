using Microsoft.AspNetCore.Http;
using Pattern_of_life.Service;
using System.Security.Claims;

namespace Pattern_of_life.Service
{
    public class ContextAccessor : IContextAccessor
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public ContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string UserId()
        {
            return httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string UserName()
        {
            return httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        }

        public string UserEmail()
        {
            return httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
        }

        public string IPAddress()
        {
            return httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}
