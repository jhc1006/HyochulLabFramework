using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace HyochulLab.Logging.Extensions;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddHyochulLabLogging(this ILoggingBuilder builder, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        builder.ClearProviders(); // 기존 로거 제거
        builder.AddSerilog();     // Serilog 사용

        return builder;
    }
}
