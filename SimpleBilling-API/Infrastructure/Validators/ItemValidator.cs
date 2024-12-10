using FluentValidation;
using SimpleBilling_API.DTOs;

namespace SimpleBilling_API.Infrastructure.Validators;

public class ItemValidator : AbstractValidator<ItemRequest>
{
    public ItemValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Item Name can not be empty!")
            .NotNull()
            .WithMessage("Item Name can not be null!")
            .MinimumLength(2)
            .WithMessage("Item Name must be at least 2 characters!")
            .MaximumLength(100)
            .WithMessage("Item Name can not exceed 100 characters!");

        RuleFor(x => x.Manufacturer)
            .NotEmpty()
            .WithMessage("Item Manufacturer can not be empty!")
            .NotNull()
            .WithMessage("Item Manufacturer can not be null!")
            .MinimumLength(4)
            .WithMessage("Item Manufacturer must be at least 4 characters!")
            .MaximumLength(60)
            .WithMessage("Item Manufacturer can not exceed 60 characters!");

        RuleFor(x => x.Price)
            .NotEmpty()
            .WithMessage("Item Price can not be empty!")
            .NotNull()
            .WithMessage("Item Price can not be null!")
            .PrecisionScale(7, 2, false)
            .GreaterThan(0.0M)
            .WithMessage("Item Price must be greater than zero!");

        RuleFor(x => x.Discount)
            .NotEmpty()
            .WithMessage("Item Discount can not be empty!")
            .NotNull()
            .WithMessage("Item Discount can not be null")
            .Must(discount => discount >= 0)
            .WithMessage("Item Discount must be greater than or equal to zero!");
    }
}
