using System.Globalization;

namespace eservices.Culture
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var cultureCookie = context.Request.Cookies["culture"];
            if (!string.IsNullOrEmpty(cultureCookie))
            {
                var cultureInfo = new CultureInfo(cultureCookie);
                CultureInfo.CurrentCulture = cultureInfo;
                CultureInfo.CurrentUICulture = cultureInfo;
            }

            await _next(context);
        }
    }

    public static class CultureMiddlewareExtensions
    {
        public static IApplicationBuilder UseCultureMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CultureMiddleware>();
        }
    }

}
