using FluentAssertions;
using Moq;
using SimpleBilling_API.Application.Commands;
using SimpleBilling_API.Application.Handlers;
using SimpleBilling_API.Interfaces;
using SimpleBilling_API.Models;

namespace SimpleBilling_API.Tests.Application.Commands;

public class RemoveItemCommandHandlerTests
{
    [Fact]
    public async Task Should_ReturnOne_WhenItemToRemoveIsFound()
    {
        // Arrange
        Mock<IItemRepository> repository = new();
        RemoveItemCommand command = new(1);
        RemoveItemCommandHandler handler = new(repository.Object);

        ServiceResponse<int> serviceResponse = new()
        {
            Data = 1,
            Message = string.Empty,
            Success = true
        };

        repository
            .Setup(x => x.RemoveItemAsync(1))
            .ReturnsAsync(serviceResponse);

        // Act
        ServiceResponse<int> result = await handler.Handle(command);

        // Assert
        result.Data.Should().Be(1);
        result.Message.Should().BeEmpty();
        result.Success.Should().BeTrue();

        repository.Verify(x => x.RemoveItemAsync(1), Times.Once);
    }
};
