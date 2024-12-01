using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SimpleBilling_API.Application.Commands;
using SimpleBilling_API.Application.Queries;
using SimpleBilling_API.DTOs;
using SimpleBilling_API.Models;
using Wolverine;

namespace SimpleBilling_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IValidator<ItemRequest> _validator;
    private readonly IMessageBus _messageBus;

    public ItemsController(IValidator<ItemRequest> validator, IMessageBus messageBus)
    {
        _validator = validator;
        _messageBus = messageBus;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddNewItemAsync([FromBody] ItemRequest newItem)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(newItem);
        if (!validationResult.IsValid)
            return BadRequest(string.Join(", ", validationResult.Errors));

        ServiceResponse<int> item = await _messageBus.InvokeAsync<ServiceResponse<int>>(new AddItemCommand(newItem));
        return item.Data == 0
            ? BadRequest(item)
            : Ok(item);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllItemsAsync()
    {
        ServiceResponse<ICollection<ItemResponse>> items = await _messageBus.InvokeAsync<ServiceResponse<ICollection<ItemResponse>>>(new GetItemsQuery());
        return items.Data != null && items.Data.Any()
            ? Ok(items)
            : NoContent();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetItemByIdAsync([FromRoute] int id)
    {
        ServiceResponse<ItemResponse> items = await _messageBus.InvokeAsync<ServiceResponse<ItemResponse>>(new GetItemByIdQuery(id));
        return items.Data != null
            ? Ok(items)
            : NotFound();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateItemAsync([FromRoute] int id, [FromBody] ItemRequest updatedItem)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(updatedItem);
        if (!validationResult.IsValid)
            return BadRequest(string.Join(", ", validationResult.Errors));

        ServiceResponse<int> item = await _messageBus.InvokeAsync<ServiceResponse<int>>(new UpdateItemCommand(id, updatedItem));
        return item.Data == 0
            ? BadRequest(item)
            : NoContent();
    }


    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveItemAsync([FromRoute] int id)
    {
        ServiceResponse<int> item = await _messageBus.InvokeAsync<ServiceResponse<int>>(new RemoveItemCommand(id));
        return item.Data == 0
            ? BadRequest(item)
            : NoContent();
    }
}
