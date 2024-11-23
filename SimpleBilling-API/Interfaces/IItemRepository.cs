using SimpleBilling_API.DTOs;
using SimpleBilling_API.Models;

namespace SimpleBilling_API.Interfaces;

public interface IItemRepository
{
    Task<ServiceResponse<ICollection<ItemResponse>>> GetAllItemsAsync();
    Task<ServiceResponse<ICollection<ItemResponse>>> GetItemByIdAsync(int id);
    Task<ServiceResponse<int>> AddItemAsync(ItemRequest newItem);
    Task<ServiceResponse<int>> UpdateItemAsync(int id, ItemRequest updatedItem);
    Task<ServiceResponse<int>> RemoveItemAsync(int id);
}
