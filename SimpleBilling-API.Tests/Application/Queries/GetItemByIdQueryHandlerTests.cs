using FluentAssertions;
using Moq;
using SimpleBilling_API.Application.Handlers;
using SimpleBilling_API.Application.Queries;
using SimpleBilling_API.DTOs;
using SimpleBilling_API.Interfaces;
using SimpleBilling_API.Models;

namespace SimpleBilling_API.Tests.Application.Queries;

public class GetItemByIdQueryHandlerTests
{
    [Fact]
    public async Task Should_ReturnSingleItem_When_ItemIsFound()
    {
        // Arrange
        Mock<IItemRepository> repository = new();
        GetItemByIdQuery query = new(1);
        GetItemByIdQueryHandler handler = new(repository.Object);
        ItemResponse item = new()
        {
            Id = 1,
            Name = "Radeon RX 580",
            Manufacturer = "AMD",
            Price = 200.00M,
            Discount = 10
        };

        ServiceResponse<ItemResponse> serviceResponse = new()
        {
            Data = item,
            Message = string.Empty,
            Success = true
        };

        repository
            .Setup(x => x.GetItemByIdAsync(1))
            .ReturnsAsync(serviceResponse);

        // Act
        ServiceResponse<ItemResponse> result = await handler.Handle(query);

        // Assert
        result.Data.Should().NotBeNull();
        result.Message.Should().BeEmpty();
        result.Success.Should().BeTrue();

        result.Data!.Id.Should().Be(1);
        result.Data.Name.Should().Be("Radeon RX 580");
        result.Data.Manufacturer.Should().Be("AMD");
        result.Data.Price.Should().Be(200.00M);
        result.Data.Discount.Should().Be(10);

        repository.Verify(x => x.GetItemByIdAsync(1), Times.Once);
    }
}
