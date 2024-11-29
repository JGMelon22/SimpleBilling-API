using FluentAssertions;
using Moq;
using SimpleBilling_API.Application.Handlers;
using SimpleBilling_API.Application.Queries;
using SimpleBilling_API.DTOs;
using SimpleBilling_API.Interfaces;
using SimpleBilling_API.Models;

namespace SimpleBilling_API.Tests.Application.Queries;

public class GetItemsQueryHandlerTests
{
    [Fact]
    public async Task Should_ReturnListOfItems_When_CollectionHasValues()
    {
        // Arrange
        Mock<IItemRepository> repository = new();
        GetItemsQuery query = new();
        GetItemsQueryHandler handler = new(repository.Object);
        ICollection<ItemResponse> items =
        [
            new()
            {
                Id = 1,
                Name = "Radeon RX 580",
                Manufacturer = "AMD",
                Price = 200.00M,
                Discount = 10
            },
            new()
            {
                Id = 2,
                Name = "Samsung Galaxy S23",
                Manufacturer = "Samsung",
                Price = 799.99M,
                Discount = 50
            },
            new()
            {
                Id = 3,
                Name = "PlayStation 5",
                Manufacturer = "Sony",
                Price = 499.99M,
                Discount = 30
            },
            new()
            {
                Id = 4,
                Name = "Apple MacBook Air M2",
                Manufacturer = "Apple",
                Price = 1099.99M,
                Discount = 100
            },
            new()
            {
                Id = 5,
                Name = "Bose QuietComfort 45",
                Manufacturer = "Bose",
                Price = 329.99M,
                Discount = 20
            }
        ];

        ServiceResponse<ICollection<ItemResponse>> serviceResponse = new()
        {
            Data = items,
            Message = string.Empty,
            Success = true
        };

        repository
            .Setup(x => x.GetAllItemsAsync())
            .ReturnsAsync(serviceResponse);

        // Act
        ServiceResponse<ICollection<ItemResponse>> result = await handler.Handle(query);

        // Item
        result.Data.Should().NotBeNull();
        result.Message.Should().BeEmpty();
        result.Success.Should().BeTrue();

        result.Data!.Count().Should().Be(5);

        repository.Verify(x => x.GetAllItemsAsync(), Times.Once);
    }
}
