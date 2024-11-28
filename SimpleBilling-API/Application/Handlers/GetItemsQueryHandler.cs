using SimpleBilling_API.Application.Queries;
using SimpleBilling_API.DTOs;
using SimpleBilling_API.Interfaces;
using SimpleBilling_API.Models;

namespace SimpleBilling_API.Application.Handlers;

public class GetItemsQueryHandler
{
    private readonly IItemRepository _repository;

    public GetItemsQueryHandler(IItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<ICollection<ItemResponse>>> Handle(GetItemsQuery query)
        => await _repository.GetAllItemsAsync();
}
