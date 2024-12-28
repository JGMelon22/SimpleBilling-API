using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleBilling_API.DTOs;
using SimpleBilling_API.Infrastructure.Repository;
using SimpleBilling_API.Models;

namespace SimpleBilling_API.IntegrationTests.Repositories;

public class ItemRepositoryTests : BaseIntegrationTest
{
    private readonly ItemRepository _repository;

    public ItemRepositoryTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
        var logger = Scope.ServiceProvider.GetRequiredService<ILogger<ItemRepository>>();
        _repository = new ItemRepository(DbContext, logger);
    }

    [Fact]
    public async Task Should_ReturnSingleItem_When_ItemIsFound()
    {
        // Arrange
        ItemResponse item = new()
        {
            Id = 2,
            Name = "Gaming Keyboard",
            Manufacturer = "Keytronics",
            Price = 75.49m,
            Discount = 10
        };

        // Act
        ServiceResponse<ItemResponse> result = await _repository.GetItemByIdAsync(2);

        // Assert
        result.Data!.Id.Should().Be(2);
        result.Data!.Name.Should().Be("Gaming Keyboard");
        result.Data!.Manufacturer.Should().Be("Keytronics");
        result.Data!.Price.Should().Be(75.49m);
        result.Data!.Discount.Should().Be(10);
        result.Success.Should().BeTrue();
        result.Message.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_ReturnSingleItem_When_ItemNameMatches()
    {
        // Arrange
        ItemResponse item = new()
        {
            Id = 7,
            Name = "4K Monitor",
            Manufacturer = "VisionMax",
            Price = 399.99m,
            Discount = 30
        };

        // Act
        ServiceResponse<ItemResponse> result = await _repository.GetItemByNameAsync("4K Monitor");

        // Assert
        result.Data!.Id.Should().Be(7);
        result.Data!.Name.Should().Be("4K Monitor");
        result.Data!.Manufacturer.Should().Be("VisionMax");
        result.Data!.Price.Should().Be(399.99m);
        result.Data!.Discount.Should().Be(30);
        result.Success.Should().BeTrue();
        result.Message.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_ReturnOne_When_InserterdItemIsValid()
    {
        // Arrange
        ItemRequest newItem = new("GeForce GTX 1660", "NVIDIA", 250.00M, 15);

        // Act
        ServiceResponse<int> result = await _repository.AddItemAsync(newItem);

        // Assert
        result.Data.Should().Be(1);
        result.Success.Should().BeTrue();
    }

    [Fact]
    public async Task Should_ReturnOne_When_ItemToUpdateIsValid()
    {
        // Arrange
        ItemRequest updatedItem = new("Radeon RX Vega 64", "AMD", 599.99M, 8);

        // Act
        ServiceResponse<int> result = await _repository.UpdateItemAsync(1, updatedItem);

        // Assert
        result.Data.Should().Be(1);
        result.Success.Should().BeTrue();
    }

    // [Fact]
    // public async Task Should_ReturnListOfItems_When_CollectionHasValues()
    // {
    //     // Act
    //     ServiceResponse<ICollection<ItemResponse>> result = await _repository.GetAllItemsAsync();

    //     // Assert
    //     result.Data.Should().NotBeNull();
    //     result.Message.Should().BeEmpty();
    //     result.Success.Should().BeTrue();

    //     result.Data!.Count().Should().Be(10);
    // }

    [Fact]
    public async Task Should_ReturnOne_WhenItemToRemoveIsFound()
    {
        // Arrange

        // Act
        ServiceResponse<int> result = await _repository.RemoveItemAsync(7);

        // Assert
        result.Data.Should().Be(1);
        result.Message.Should().BeEmpty();
        result.Success.Should().BeTrue();
    }
}
