using FluentAssertions;
using Moq;
using SimpleBilling_API.Application.Handlers;
using SimpleBilling_API.Application.Queries;
using SimpleBilling_API.DTOs;
using SimpleBilling_API.Interfaces;
using SimpleBilling_API.Models;

namespace SimpleBilling_API.Tests.Application.Queries;

public class GetCartsItemsByNamesQueryTests
{
    [Fact]
    public async Task Should_ReturnCollectionOfNamesInCart_WhenNameExists()
    {
        // Arrange
        Mock<IItemRepository> repository = new();
        GetCartsItemsByNamesQuery query = new([
            new CartItem
            {
                Name = "iPhone 12 Pro Max",
                Quantity = 2
            },
            new CartItem
            {
                Name = "Samsung Galaxy S24",
                Quantity = 1
            },
            new CartItem
            {
                Name = "MacBook Pro",
                Quantity = 3
            }
        ]);
        GetCartsItemsByNamesQueryHandler handler = new(repository.Object);
        ICollection<ItemResponse> cartItems =
        [
            new()
            {
                Id = 1,
                Name = "iPhone 12 Pro Max",
                Manufacturer = "Apple",
                Price = 1099.99M,
                Discount = 10
            },
            new ()
            {
                Id = 2,
                Name = "Samsung Galaxy S24",
                Manufacturer = "Samsung",
                Price = 999.99M,
                Discount = 5
            },
            new ()
            {
                Id = 3,
                Name = "MacBook Pro",
                Manufacturer = "Apple",
                Price = 1999.99M,
                Discount = 15
            }
        ];

        ServiceResponse<ICollection<ItemResponse>> serviceResponse = new()
        {
            Data = cartItems,
            Message = string.Empty,
            Success = true
        };

        repository
            .Setup(x => x.GetCartsItemsByNamesAsync(query.CartItems))
            .ReturnsAsync(serviceResponse);

        // Act
        ServiceResponse<ICollection<ItemResponse>> result = await handler.Handle(query);

        // Assert
        result.Data.Should().NotBeNull();
        result.Message.Should().BeEmpty();
        result.Success.Should().BeTrue();

        result.Data!.Count().Should().Be(3);

        repository.Verify(x => x.GetCartsItemsByNamesAsync(query.CartItems), Times.Once);
    }
}
