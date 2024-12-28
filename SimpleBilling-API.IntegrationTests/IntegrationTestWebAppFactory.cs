using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using SimpleBilling_API.Infrastructure.Data;
using Testcontainers.PostgreSql;

namespace SimpleBilling_API.IntegrationTests;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;

    public IntegrationTestWebAppFactory()
    {
        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:17.2-alpine3.21")
            .WithPassword("Melon@123")
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // builder.UseEnvironment("Testing");

        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DapperDbContext));

            if (descriptor != null)
                services.Remove(descriptor);

            services.AddSingleton(x =>
            {
                return new DapperDbContext(new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        { "ConnectionStrings:Default", _dbContainer.GetConnectionString() }
                    })
                    .Build());
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await InitializeDatabase();
    }

    private async Task InitializeDatabase()
    {
        try
        {
            using var scope = Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DapperDbContext>();
            using var connection = dbContext.CreateConnection();

            await connection.ExecuteAsync("""
                CREATE TABLE items (
                    id SERIAL NOT NULL,
                    name VARCHAR(100) NOT NULL,
                    manufacturer VARCHAR(100) NULL,
                    price DECIMAL(7, 2) NOT NULL,
                    discount REAL NOT NULL,
                    CONSTRAINT pk_item PRIMARY KEY (id)
                );
            """);

            await connection.ExecuteAsync("""
                CREATE UNIQUE INDEX IF NOT EXISTS "idx_item_id"
                ON items (id);
            """);

            await connection.ExecuteAsync("""
                INSERT INTO items(name, manufacturer, price, discount)
                VALUES
                    ('Wireless Mouse', 'TechGear', 25.99, 5.00),
                    ('Gaming Keyboard', 'Keytronics', 75.49, 10.00),
                    ('Smartphone', 'MobileTech', 699.00, 50.00),
                    ('Noise Cancelling Headphones', 'SoundHub', 149.99, 20.00),
                    ('USB-C Charging Cable', 'ChargePro', 12.99, 2.00),
                    ('External Hard Drive', 'DataVault', 89.99, 15.00),
                    ('4K Monitor', 'VisionMax', 399.99, 30.00),
                    ('Bluetooth Speaker', 'AudioWave', 49.99, 10.00),
                    ('Fitness Tracker', 'HealthSync', 129.99, 25.00),
                    ('Smartwatch', 'TimeTech', 199.99, 40.00);
            """);
        }

        catch (Exception ex)
        {
            throw new Exception("An error occurred while initializing the database.", ex);
        }
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }

    public DapperDbContext GetDbContext()
    {
        var scope = Services.CreateScope();
        return scope.ServiceProvider
            .GetRequiredService<DapperDbContext>();
    }
}
