using SimpleBilling_API.Application.Queries;
using SimpleBilling_API.DTOs;
using SimpleBilling_API.Interfaces;
using SimpleBilling_API.Models;

namespace SimpleBilling_API.Application.Handlers;


public class GetItemByNameQueryHandler
{
    private readonly IItemRepository _repository;

    public GetItemByNameQueryHandler(IItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<ItemResponse>> Handle(GetItemByNameQuery query)
        => await _repository.GetItemByNameAsync(query.Name);
}
