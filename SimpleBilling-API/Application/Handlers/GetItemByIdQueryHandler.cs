using SimpleBilling_API.Application.Queries;
using SimpleBilling_API.DTOs;
using SimpleBilling_API.Interfaces;
using SimpleBilling_API.Models;

namespace SimpleBilling_API.Application.Handlers;

public class GetItemByIdQueryHandler
{
    private readonly IItemRepository _repository;

    public GetItemByIdQueryHandler(IItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<ItemResponse>> Handle(GetItemByIdQuery query)
        => await _repository.GetItemByIdAsync(query.Id);
}
