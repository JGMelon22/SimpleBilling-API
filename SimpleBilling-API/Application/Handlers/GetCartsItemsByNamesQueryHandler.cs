using SimpleBilling_API.Application.Queries;
using SimpleBilling_API.DTOs;
using SimpleBilling_API.Interfaces;
using SimpleBilling_API.Models;

namespace SimpleBilling_API.Application.Handlers;

public class GetCartsItemsByNamesQueryHandler
{
    private readonly IItemRepository _repository;

    public GetCartsItemsByNamesQueryHandler(IItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<ICollection<ItemResponse>>> Handle(GetCartsItemsByNamesQuery query)
        => await _repository.GetCartsItemsByNamesAsync(query.CartItems);
}
