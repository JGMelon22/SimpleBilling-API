using FluentAssertions;
using FluentValidation.Results;
using SimpleBilling_API.DTOs;
using SimpleBilling_API.Infrastructure.Validators;

namespace SimpleBilling_API.Tests.Validators;

public class ItemValidatorTests
{
    [Fact]
    public async Task Should_BeValid_WhenItemRequestIsCorrectltInformmed()
    {
        // Arrange
        ItemRequest item = new("Boeing 747", "Boeing", 99_999.0M, 12);
        ItemValidator validator = new();

        // Act
        ValidationResult result = await validator.ValidateAsync(item);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Should_BeInvalid_WhenItemRequestHasNoNameNorBrand()
    {
        // Arrange
        ItemRequest item = new(string.Empty, string.Empty, 99_999.0M, 12);
        ItemValidator validator = new();

        // Act
        ValidationResult result = await validator.ValidateAsync(item);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Name" && x.ErrorMessage == "Item Name can not be empty!");
        result.Errors.Should().Contain(x => x.PropertyName == "Manufacturer" && x.ErrorMessage == "Item Manufacturer can not be empty!");
    }

    [Fact]
    public async Task Should_BeInvalid_WhenItemRequestDiscountIsEqualToZeroAndPriceEqualsToZero()
    {
        // Arrange
        ItemRequest item = new("Airbus A350 XWB", "Airbus", 0.0M, 5);
        ItemValidator validator = new();

        // Act
        ValidationResult result = await validator.ValidateAsync(item);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Price" && x.ErrorMessage == "Item Price must be greater than zero!");
    }
}
