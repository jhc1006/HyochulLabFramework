using HyochulLab.Web.Middleware;
using Microsoft.AspNetCore.Builder;

namespace HyochulLab.Web.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseHyochulLabExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<HyochulLab.Web.Middleware.ExceptionHandlingMiddleware>();
    }

    public static IApplicationBuilder UseHyochulLabRequestLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestLoggingMiddleware>();
    }
}
