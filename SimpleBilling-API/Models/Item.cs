namespace SimpleBilling_API.Models;

public class Item
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Manufacturer { get; set; }
    public decimal Price { get; set; }
    public byte Discount { get; set; }
}
