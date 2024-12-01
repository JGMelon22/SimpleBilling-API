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

    /// <summary>
    /// Includes a new Brand
    /// </summary>
    /// <param name="newItem"></param>
    /// <returns>A newly Ok Object Response when item request is valid</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/Items
    ///     {
    ///        "name": "Gaming Chair",
    ///        "manufacturer": "DX Racing",
    ///        "price": "299",
    ///        "discount": "15"
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Returns Ok Object Response when item request is valid.</response>
    /// <response code="400">If item is request is bad formmated.</response>
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

    /// <summary>
    /// Returns a list of Items
    /// </summary>
    /// <response code="200">Returns a list of items.</response>
    /// <response code="204">If the items list is empty.</response>
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

    /// <summary>
    /// Search and return a single Item
    /// </summary>
    /// <param name="id"></param>
    /// <returns>A single Item</returns>
    /// <response code="200">Returns the found item.</response>
    /// <response code="404">If item with provided id not found.</response>
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

    /// <summary>
    /// Updates an item content
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updatedItem"></param>
    /// <returns>	</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     PATCH /api/Brands
    ///     {
	///        "name": "Gamin Mouse",
	///         "manufacturer": "Logitech",
	///         "price": 100.25,
	///         "discount": 15
	///     }
    ///
    /// </remarks>
    /// <response code="204">Successfully updated an item.</response>
    /// <response code="400">If item request is bad formmated.</response>
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

    /// <summary>
    /// Removes an Item
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="204">Successfully removed an item</response>
    /// <response code="400">Item to be remove not found</response>
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
