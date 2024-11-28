using SimpleBilling_API.Application.Commands;
using SimpleBilling_API.Interfaces;
using SimpleBilling_API.Models;

namespace SimpleBilling_API.Application.Handlers;

public class RemoveItemCommandHandler
{
    private readonly IItemRepository _repository;

    public RemoveItemCommandHandler(IItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<int>> Handle(RemoveItemCommand command)
        => await _repository.RemoveItemAsync(command.Id);
}
