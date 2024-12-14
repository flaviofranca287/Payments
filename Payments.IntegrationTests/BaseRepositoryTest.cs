using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StrangerData;
using StrangerData.SqlServer;
using System;
using Payments.Infrastructure;
using Payments.WebApi;


namespace Payments.IntegrationTests;

public class BaseRepositoryTest : IDisposable
{
    protected readonly DatabaseContext DatabaseContext;
    protected readonly string? DatabaseConnectionString;
    protected readonly DataFactory<SqlServerDialect> DataFactory;
    private readonly Random _random = new();

    protected BaseRepositoryTest()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json")
            .Build();

        var server = new TestServer(new WebHostBuilder()
            .UseStartup<Startup>()
            .ConfigureAppConfiguration(cfg => cfg.AddConfiguration(configuration)));

        var serviceProvider = server.Services;
        var databaseSettings = serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>();
        DatabaseContext = new DatabaseContext(databaseSettings);

        DatabaseConnectionString = configuration["ConnectionStrings:LocalHost"];
        DataFactory = new DataFactory<SqlServerDialect>(DatabaseConnectionString);
    }
    
    
    public void Dispose()
    {
        DataFactory.TearDown();
    }
}