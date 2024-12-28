using Microsoft.Extensions.DependencyInjection;
using SimpleBilling_API.Infrastructure.Data;
using Xunit;

namespace SimpleBilling_API.IntegrationTests;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    protected readonly IntegrationTestWebAppFactory Factory;
    protected readonly DapperDbContext DbContext;
    protected readonly IServiceScope Scope;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        Factory = factory;
        Scope = Factory.Services.CreateScope();
        DbContext = Factory.GetDbContext();
    }
}
