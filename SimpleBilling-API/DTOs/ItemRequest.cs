namespace SimpleBilling_API.DTOs;

public record ItemRequest(string Name, string Manufacturer, decimal Price, byte Discount);
