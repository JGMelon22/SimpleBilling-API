using SimpleBilling_API.Models;

namespace SimpleBilling_API.Application.Queries;

public record GetCartsItemsByNamesQuery(ICollection<CartItem> CartItems);
