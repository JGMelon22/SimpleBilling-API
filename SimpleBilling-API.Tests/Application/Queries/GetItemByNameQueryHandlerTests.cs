using FluentAssertions;
using Moq;
using SimpleBilling_API.Application.Handlers;
using SimpleBilling_API.Application.Queries;
using SimpleBilling_API.DTOs;
using SimpleBilling_API.Interfaces;
using SimpleBilling_API.Models;

namespace SimpleBilling_API.Tests.Application.Queries;

public class GetItemByNameQueryHandlerTests
{
    [Theory]
    [InlineData("Samsung Galaxy S24", true, "")]
    [InlineData("iPhone 12 Pro Max", false, "Item with name 12 Pro Max found!")]
    public async Task Should_ReturnSingleItem_When_ItemNameMatches(string name, bool expectedResult, string errorMessage)
    {
        // Arrange
        Mock<IItemRepository> repository = new();
        GetItemByNameQuery query = new(name);
        GetItemByNameQueryHandler handler = new(repository.Object);
        ItemResponse item = new()
        {
            Id = 1,
            Name = name,
            Manufacturer = "Samsung",
            Price = 799.99M,
            Discount = 5
        };

        ServiceResponse<ItemResponse> serviceResponse = new()
        {
            Data = item,
            Message = errorMessage,
            Success = expectedResult
        };

        repository
            .Setup(x => x.GetItemByNameAsync(name))
            .ReturnsAsync(serviceResponse);

        // Act
        ServiceResponse<ItemResponse> result = await handler.Handle(query);

        // Assert
        result.Data.Should().NotBeNull();
        result.Message.Should().Be(errorMessage);
        result.Success.Should().Be(expectedResult);

        result.Data!.Id.Should().Be(1);
        result.Data.Name.Should().Contain(name);
        result.Data.Manufacturer.Should().Be("Samsung");
        result.Data.Price.Should().Be(799.99M);
        result.Data.Discount.Should().Be(5);

        repository.Verify(x => x.GetItemByNameAsync(name), Times.Once);
    }
}
