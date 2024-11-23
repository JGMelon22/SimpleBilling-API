using SimpleBilling_API.DTOs;
using SimpleBilling_API.Models;

namespace SimpleBilling_API.Infrastructure.Mapper;

public static class ItemMapper
{
    public static ItemResponse ItemToItemResponse(this Item item)
    {
        return new ItemResponse
        {
            Id = item.Id,
            Name = item.Name,
            Manufacturer = item.Manufacturer,
            Price = item.Price,
            Discount = item.Discount
        };
    }

    public static Item ItemRequestToItem(this ItemRequest item)
    {
        return new Item
        {
            Name = item.Name,
            Manufacturer = item.Manufacturer,
            Price = item.Price,
            Discount = item.Discount
        };
    }
}
