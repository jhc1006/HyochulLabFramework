using HyochulLab.Web.Extensions;
using HyochulLab.Web.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ModelValidationFilter>();
});

var app = builder.Build();

app.UseHyochulLabExceptionHandling();
app.MapControllers();

app.MapGet("/", () => "HyochulLab 웹앱 정상 작동 중 ✅");

app.Run();
