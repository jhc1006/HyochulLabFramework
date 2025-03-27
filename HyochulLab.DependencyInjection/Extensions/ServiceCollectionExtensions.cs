using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HyochulLab.Data.Context;
using HyochulLab.Data.Interfaces;
using HyochulLab.Data.UnitOfWork;
using HyochulLab.Web.Filters;
using Microsoft.AspNetCore.Mvc;


namespace HyochulLab.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHyochulLabFramework(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // 1. Add Controllers + ModelValidationFilter
        services.AddControllers(options =>
        {
            options.Filters.Add<ModelValidationFilter>();
        });

        // 2. Add DbContext
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        // 3. Add UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // 4. (예정) Add Logging, Caching, Auth 등...

        return services;
    }
}
