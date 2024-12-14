using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payments.Infrastructure;

namespace Payments.WebApi.Extensions;

public static class DatabaseSetup
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("LocalHost");

        services.Configure<DatabaseSettings>(options => options.ConnectionString = connectionString);

        services.AddScoped<IDatabaseContext, DatabaseContext>();

        return services;
    }
    
    
}