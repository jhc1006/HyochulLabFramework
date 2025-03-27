using HealthChecks.UI.Client;
using HyochulLab.Caching.Extensions;
using HyochulLab.Data.Context;
using HyochulLab.DependencyInjection.Extensions;
using HyochulLab.Web.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog 설정
builder.Host.UseSerilog((context, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext());

// 프레임워크 구성
builder.Services.AddHyochulLabFramework(builder.Configuration);

// 캐싱 모듈
builder.Services.AddHyochulLabCaching();

// HealthChecks 설정
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>("Database"); // DB 연결 상태 체크
builder.Services.AddHealthChecksUI()
    .AddInMemoryStorage();

// API 버전 관리 설정
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// SwaggerGen 설정 (JWT 인증 정의)
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter the JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    };

    options.AddSecurityRequirement(securityRequirement);
});

// JWT 인증 및 설정
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// 로그 기록 미들웨어
app.UseHyochulLabRequestLogging();

// 예외 처리 미들웨어
app.UseHyochulLabExceptionHandling();

// Health Checks 엔드포인트
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Health Checks UI 엔드포인트
app.MapHealthChecksUI(options =>
{
    options.UIPath = "/healthchecks-ui";
});

// Swagger 설정
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "HyochulLab API v1");
});

// 컨트롤러 등록
app.MapControllers();

// 기본 루트 요청
app.MapGet("/", () => "로깅 테스트!");

app.Run();
