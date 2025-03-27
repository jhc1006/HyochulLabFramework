using Microsoft.AspNetCore.Builder;

namespace HyochulLab.Web.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseHyochulLabExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<HyochulLab.Web.Middleware.ExceptionHandlingMiddleware>();
    }
}
