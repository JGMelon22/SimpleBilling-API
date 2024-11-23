namespace SimpleBilling_API.DTOs;

public record ItemRequest(string Name, decimal Price, byte Discount);
