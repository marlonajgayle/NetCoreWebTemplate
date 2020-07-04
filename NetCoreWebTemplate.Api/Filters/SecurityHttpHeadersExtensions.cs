using Microsoft.AspNetCore.Builder;

namespace NetCoreWebTemplate.Api.Filters
{
    public static class SecurityHttpHeadersExtensions
    {
        public static IApplicationBuilder UseHttpSecurityHeaders(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SecurityHttpHeaders>();
        }
    }
}
