using FluentAssertions;
using Moq;
using SimpleBilling_API.Application.Commands;
using SimpleBilling_API.Application.Handlers;
using SimpleBilling_API.DTOs;
using SimpleBilling_API.Interfaces;
using SimpleBilling_API.Models;

namespace SimpleBilling_API.Tests.Application.Commands;

public class UpdateItemCommandHandlerTests
{
    [Fact]
    public async Task Should_ReturnOne_When_ItemToUpdateIsValid()
    {
        // Act
        Mock<IItemRepository> repository = new();
        ItemRequest updatedItem = new("Radeon RX Vega 64", "AMD", 599.99M, 8);
        UpdateItemCommand command = new(1, updatedItem);
        UpdateItemCommandHandler handler = new(repository.Object);

        ServiceResponse<int> serviceResponse = new()
        {
            Data = 1,
            Message = string.Empty,
            Success = true
        };

        repository
            .Setup(x => x.UpdateItemAsync(1, updatedItem))
            .ReturnsAsync(serviceResponse);

        // Act
        ServiceResponse<int> result = await handler.Handle(command);

        // Assert
        result.Data.Should().Be(1);
        result.Message.Should().BeEmpty();
        result.Success.Should().BeTrue();

        repository.Verify(x => x.UpdateItemAsync(1, updatedItem), Times.Once);
    }
}
