using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HyochulLab.Web.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        // 요청 로깅
        var request = context.Request;
        var requestBodyContent = await ReadRequestBody(request);
        _logger.LogInformation("HTTP {Method} {Path} | Request Body: {RequestBody}",
            request.Method, request.Path, requestBodyContent);

        // 응답 로깅을 위한 스트림 교체
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        var sw = Stopwatch.StartNew();

        await _next(context);

        sw.Stop();

        // 응답 로깅
        var responseBodyContent = await ReadResponseBody(context.Response);
        _logger.LogInformation("HTTP {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds}ms | Response Body: {ResponseBody}",
            request.Method, request.Path, context.Response.StatusCode, sw.ElapsedMilliseconds, responseBodyContent);

        await responseBody.CopyToAsync(originalBodyStream);
    }

    private static async Task<string> ReadRequestBody(HttpRequest request)
    {
        request.EnableBuffering();

        var body = await new StreamReader(request.Body, Encoding.UTF8, true, 1024, true).ReadToEndAsync();
        request.Body.Position = 0;

        return body;
    }

    private static async Task<string> ReadResponseBody(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);

        return text;
    }
}
