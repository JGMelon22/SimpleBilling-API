using SimpleBilling_API.Models.Enums;

namespace SimpleBilling_API.Models;

public class Cart
{
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public Currency Currency { get; set; }
}
