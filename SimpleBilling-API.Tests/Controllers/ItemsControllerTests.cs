using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SimpleBilling_API.Application.Commands;
using SimpleBilling_API.Application.Queries;
using SimpleBilling_API.Controllers;
using SimpleBilling_API.DTOs;
using SimpleBilling_API.Models;
using Wolverine;

namespace SimpleBilling_API.Tests.Controllers;

public class ItemsControllerTests
{
    [Fact]
    public async Task Should_ReturnSucess_When_NewItemIsValid()
    {
        // Arrange
        Mock<IValidator<ItemRequest>> validator = new();
        Mock<IMessageBus> messageBus = new();
        ValidationResult validationResult = new();
        ItemsController controller = new(validator.Object, messageBus.Object);
        ItemRequest newItem = new("Ryzen 9 9900X", "AMD", 409.00M, 12);

        ServiceResponse<int> serviceResponse = new()
        {
            Data = 1,
            Message = string.Empty,
            Success = true
        };

        validator
            .Setup(x => x.ValidateAsync(newItem, default))
            .ReturnsAsync(validationResult);

        messageBus
            .Setup(x => x.InvokeAsync<ServiceResponse<int>>(
                It.Is<AddItemCommand>(cmd => cmd.NewItem == newItem),
                It.IsAny<CancellationToken>(),
                default))
            .ReturnsAsync(serviceResponse);

        // Act
        IActionResult result = await controller.AddNewItemAsync(newItem);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        serviceResponse.Data.Should().Be(1);

        validator.Verify(x => x.ValidateAsync(newItem, It.IsAny<CancellationToken>()), Times.Once);
        messageBus.Verify(x => x.InvokeAsync<ServiceResponse<int>>(
            It.Is<AddItemCommand>(cmd => cmd.NewItem == newItem),
            It.IsAny<CancellationToken>(),
            default), Times.Once);
    }

    [Fact]
    public async Task Should_Success_When_CollecionOfItemsIsNotEmpty()
    {
        // Arrange
        Mock<IValidator<ItemRequest>> validator = new();
        Mock<IMessageBus> messageBus = new();
        ItemsController controller = new(validator.Object, messageBus.Object);
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

        messageBus
            .Setup(x => x.InvokeAsync<ServiceResponse<ICollection<ItemResponse>>>(
                It.IsAny<GetItemsQuery>(),
                It.IsAny<CancellationToken>(),
                default))
            .ReturnsAsync(serviceResponse);

        // Act
        IActionResult result = await controller.GetAllItemsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        serviceResponse.Data.Should().BeEquivalentTo(items);

        messageBus.Verify(x => x.InvokeAsync<ServiceResponse<ICollection<ItemResponse>>>(
            It.IsAny<GetItemsQuery>(),
            It.IsAny<CancellationToken>(),
            default), Times.Once);
    }

    [Fact]
    public async Task Should_ReturnCollecionOfItems_When_ItemWithProvidedIdIsFound()
    {
        // Arrange
        Mock<IValidator<ItemRequest>> validator = new();
        Mock<IMessageBus> messageBus = new();
        ItemsController controller = new(validator.Object, messageBus.Object);
        ItemResponse item = new()
        {
            Id = 1,
            Name = "Vega 64",
            Manufacturer = "AMD",
            Price = 299.00M,
            Discount = 20
        };

        ServiceResponse<ItemResponse> serviceResponse = new()
        {
            Data = item,
            Message = string.Empty,
            Success = true
        };

        messageBus
            .Setup(x => x.InvokeAsync<ServiceResponse<ItemResponse>>(
                It.IsAny<GetItemByIdQuery>(),
                It.IsAny<CancellationToken>(),
                default))
            .ReturnsAsync(serviceResponse);

        // Act
        IActionResult result = await controller.GetItemByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        serviceResponse.Data.Should().BeEquivalentTo(item);

        messageBus.Verify(x => x.InvokeAsync<ServiceResponse<ItemResponse>>(
            It.IsAny<GetItemByIdQuery>(),
            It.IsAny<CancellationToken>(),
            default), Times.Once);
    }

    [Fact]
    public async Task Should_ReturnSucess_When_ItemToUpdateIsFoundAndValidInput()
    {
        // Arrange
        Mock<IValidator<ItemRequest>> validator = new();
        Mock<IMessageBus> messageBus = new();
        ValidationResult validationResult = new();
        ItemsController controller = new(validator.Object, messageBus.Object);
        ItemRequest updatedItem = new("Core Ultra 9 285K", "Intel", 589.00M, 5);

        ServiceResponse<int> serviceResponse = new()
        {
            Data = 1,
            Message = string.Empty,
            Success = true
        };

        validator
            .Setup(x => x.ValidateAsync(updatedItem, default))
            .ReturnsAsync(validationResult);

        messageBus
            .Setup(x => x.InvokeAsync<ServiceResponse<int>>(
                It.Is<UpdateItemCommand>(cmd => cmd.Id == 1 && cmd.UpdatedItem == updatedItem),
                It.IsAny<CancellationToken>(),
                default))
            .ReturnsAsync(serviceResponse);

        // Act
        IActionResult result = await controller.UpdateItemAsync(1, updatedItem);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<NoContentResult>();
        serviceResponse.Data.Should().Be(1);

        validator.Verify(x => x.ValidateAsync(updatedItem, It.IsAny<CancellationToken>()), Times.Once);
        messageBus.Verify(x => x.InvokeAsync<ServiceResponse<int>>(
            It.Is<UpdateItemCommand>(cmd => cmd.Id == 1 && cmd.UpdatedItem == updatedItem),
            It.IsAny<CancellationToken>(),
            default), Times.Once);
    }

    [Fact]
    public async Task Should_ReturnSucess_When_ItemToBeRemovedIsFound()
    {
        // Arrange
        Mock<IValidator<ItemRequest>> validator = new();
        Mock<IMessageBus> messageBus = new();
        ItemsController controller = new(validator.Object, messageBus.Object);

        ServiceResponse<int> serviceResponse = new()
        {
            Data = 1,
            Message = string.Empty,
            Success = true
        };

        messageBus
            .Setup(x => x.InvokeAsync<ServiceResponse<int>>(
                It.Is<RemoveItemCommand>(cmd => cmd.Id == 1),
                It.IsAny<CancellationToken>(),
                default))
            .ReturnsAsync(serviceResponse);

        // Act
        IActionResult result = await controller.RemoveItemAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<NoContentResult>();
        serviceResponse.Data.Should().Be(1);

        messageBus.Verify(x => x.InvokeAsync<ServiceResponse<int>>(
            It.Is<RemoveItemCommand>(cmd => cmd.Id == 1),
            It.IsAny<CancellationToken>(),
            default), Times.Once);
    }

    [Fact]
    public async Task Should_OkResult_When_ItemNameIsFound()
    {
        // Arrange
        Mock<IValidator<ItemRequest>> validator = new();
        Mock<IMessageBus> messageBus = new();
        ItemsController controller = new(validator.Object, messageBus.Object);
        ItemResponse item = new()
        {
            Id = 1,
            Name = "iPhone X",
            Manufacturer = "Apple",
            Price = 699.00M,
            Discount = 20
        };

        ServiceResponse<ItemResponse> serviceResponse = new()
        {
            Data = item,
            Message = string.Empty,
            Success = true
        };

        messageBus
            .Setup(x => x.InvokeAsync<ServiceResponse<ItemResponse>>(
                It.IsAny<GetItemByNameQuery>(),
                It.IsAny<CancellationToken>(),
                default
            ))
            .ReturnsAsync(serviceResponse); ;

        // Act
        IActionResult result = await controller.GetItemByNameAsync("iPhone X");

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        serviceResponse.Data.Should().BeEquivalentTo(item);

        messageBus.Verify(x => x.InvokeAsync<ServiceResponse<ItemResponse>>(
            It.IsAny<GetItemByNameQuery>(),
            It.IsAny<CancellationToken>(),
            default), Times.Once);
    }
}
