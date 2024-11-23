namespace SimpleBilling_API.DTOs;

public record Item
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Manufacturer { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public byte Discount { get; init; }
}
