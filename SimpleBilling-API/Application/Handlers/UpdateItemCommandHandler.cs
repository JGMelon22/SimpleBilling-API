using SimpleBilling_API.Application.Commands;
using SimpleBilling_API.Interfaces;
using SimpleBilling_API.Models;

namespace SimpleBilling_API.Application.Handlers;

public class UpdateItemCommandHandler
{
    private readonly IItemRepository _repository;

    public UpdateItemCommandHandler(IItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<int>> Handle(UpdateItemCommand command)
        => await _repository.UpdateItemAsync(command.Id, command.UpdatedItem);
}
