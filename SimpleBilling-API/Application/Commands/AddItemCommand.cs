using SimpleBilling_API.DTOs;

namespace SimpleBilling_API.Application.Commands;

public record AddItemCommand(ItemRequest NewItem);
