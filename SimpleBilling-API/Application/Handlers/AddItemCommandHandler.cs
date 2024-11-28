using SimpleBilling_API.Application.Commands;
using SimpleBilling_API.Interfaces;
using SimpleBilling_API.Models;

namespace SimpleBilling_API.Application.Handlers;

public class AddItemCommandHandler
{
    private readonly IItemRepository _repository;

    public AddItemCommandHandler(IItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<int>> Handle(AddItemCommand command)
        => await _repository.AddItemAsync(command.NewItem);
}
