using FluentAssertions;
using Moq;
using SimpleBilling_API.Application.Commands;
using SimpleBilling_API.Application.Handlers;
using SimpleBilling_API.DTOs;
using SimpleBilling_API.Interfaces;
using SimpleBilling_API.Models;

namespace SimpleBilling_API.Tests.Application.Commands;

public class AddItemCommandHandlerTests
{
    [Fact]
    public async Task Should_ReturnOne_When_ItemIsValid()
    {
        // Arrange
        Mock<IItemRepository> repository = new();
        ItemRequest newItem = new("GeForce GTX 1660", "NVIDIA", 250.00M, 15);
        // ItemResponse item = new()
        // {
        //     Id = 1,
        //     Name = "GeForce GTX 1660",
        //     Manufacturer = "NVIDIA",
        //     Price = 250.00M,
        //     Discount = 15
        // };
        AddItemCommand command = new(newItem);
        AddItemCommandHandler handler = new(repository.Object);

        ServiceResponse<int> serviceResponse = new()
        {
            Data = 1,
            Message = string.Empty,
            Success = true
        };

        repository
            .Setup(x => x.AddItemAsync(newItem))
            .ReturnsAsync(serviceResponse);

        // Act
        ServiceResponse<int> result = await handler.Handle(command);

        // Assert
        result.Data.Should().Be(1);
        result.Message.Should().BeEmpty();
        result.Success.Should().BeTrue();

        repository.Verify(x => x.AddItemAsync(newItem), Times.Once);
    }
}
