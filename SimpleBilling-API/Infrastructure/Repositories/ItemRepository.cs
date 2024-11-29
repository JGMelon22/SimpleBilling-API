using System.Data;
using System.Reflection;
using Dapper;
using SimpleBilling_API.DTOs;
using SimpleBilling_API.Infrastructure.Data;
using SimpleBilling_API.Infrastructure.Mapper;
using SimpleBilling_API.Interfaces;
using SimpleBilling_API.Models;

namespace SimpleBilling_API.Infrastructure.Repository;

public class ItemRepository : IItemRepository
{
    private readonly DapperDbContext _context;
    private readonly ILogger<ItemRepository> _logger;

    public ItemRepository(DapperDbContext context, ILogger<ItemRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ServiceResponse<int>> AddItemAsync(ItemRequest newItem)
    {
        ServiceResponse<int> serviceResponse = new();

        string sql = """
                    INSERT INTO items(name, manufacturer, price, discount)
                    VALUES(@Name, @Manufacturer, @Price, @Discount);
                    """;

        try
        {
            string methodNameLog = $"[{GetType().Name} -> {MethodBase.GetCurrentMethod()!.ReflectedType!.Name}]";

            using (IDbConnection connection = _context.CreateConnection())
            {
                Item item = ItemMapper.ItemRequestToItem(newItem);
                int itemResult = await connection.ExecuteAsync(sql, item);

                _logger.LogInformation("{MethodName} {ObjectNameName}: {item}", methodNameLog, nameof(item), item);

                if (itemResult == 0)
                    throw new Exception("An error ocurred while inserting a new item.");

                serviceResponse.Data = itemResult;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error ocurred in {MethodName}: {Message}", $"{GetType().Name} -> {MethodBase.GetCurrentMethod()?.Name}", ex.Message);

            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<ICollection<ItemResponse>>> GetAllItemsAsync()
    {
        ServiceResponse<ICollection<ItemResponse>> serviceResponse = new();
        string sql = """
                    SELECT items AS Items,
                           name AS Name,
                           manufacturer AS Manufacturer,
                           price AS Price,
                           discount AS Discount
                    FROM items;
                    """;

        try
        {
            string methodNameLog = $"[{GetType().Name} -> {MethodBase.GetCurrentMethod()!.ReflectedType!.Name}]";

            using (IDbConnection connection = _context.CreateConnection())
            {
                IEnumerable<Item> items = await connection.QueryAsync<Item>(sql);
                ICollection<ItemResponse> itemsResponse = items.Select(x => ItemMapper.ItemToItemResponse(x)).ToList();

                _logger.LogInformation("{MethodName} {ObjectNameName}: {items}", methodNameLog, nameof(items), items);

                serviceResponse.Data = itemsResponse;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error ocurred in {MethodName}: {Message}", $"{GetType().Name} -> {MethodBase.GetCurrentMethod()?.Name}", ex.Message);

            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<ItemResponse>> GetItemByIdAsync(int id)
    {
        ServiceResponse<ItemResponse> serviceResponse = new();
        string sql = """
                    SELECT id AS Id,
                           name AS Name,
                           manufacturer AS Manufacturer,
                           price AS Price,
                           discount AS Discount
                    FROM items
                    WHERE id = @Id;
                    """;

        try
        {
            string methodNameLog = $"[{GetType().Name} -> {MethodBase.GetCurrentMethod()!.ReflectedType!.Name}]";

            using (IDbConnection connection = _context.CreateConnection())
            {
                Item? item = await connection.QueryFirstOrDefaultAsync<Item>(sql, new { Id = id });
                Item itemResponse = item
                    ?? throw new Exception($"Item with id {id} not found!");

                _logger.LogInformation("{MethodName} {ObjectNameName}: {item}", methodNameLog, nameof(item), item);

                serviceResponse.Data = ItemMapper.ItemToItemResponse(itemResponse);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error ocurred in {MethodName}: {Message}", $"{GetType().Name} -> {MethodBase.GetCurrentMethod()?.Name}", ex.Message);

            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<int>> RemoveItemAsync(int id)
    {
        ServiceResponse<int> serviceResponse = new();
        string sql = """
                    DELETE
                    FROM items
                    WHERE id = @Id
                    """;

        try
        {
            string methodNameLog = $"[{GetType().Name} -> {MethodBase.GetCurrentMethod()!.ReflectedType!.Name}]";

            using (IDbConnection connection = _context.CreateConnection())
            {
                int itemResult = await connection.ExecuteAsync(sql, new { Id = id });

                _logger.LogInformation("{MethodName} {ObjectNameName}: {itemResult}", methodNameLog, nameof(itemResult), itemResult);

                if (itemResult == 0)
                    throw new Exception($"Video Game with id {id} not found!");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error ocurred in {MethodName}: {Message}", $"{GetType().Name} -> {MethodBase.GetCurrentMethod()?.Name}", ex.Message);

            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<int>> UpdateItemAsync(int id, ItemRequest updatedItem)
    {
        ServiceResponse<int> serviceResponse = new();
        string sql = """
                    UPDATE items
                    SET name = @Name,
                        manufacturer = @Manufacturer,
                        price = @Price,
                        discount = @Discount
                    WHERE id = @Id;
                    """;

        try
        {
            string methodNameLog = $"[{GetType().Name} -> {MethodBase.GetCurrentMethod()!.ReflectedType!.Name}]";

            using (IDbConnection connection = _context.CreateConnection())
            {
                Item item = ItemMapper.ItemRequestToItem(updatedItem);
                item.Id = id;

                _logger.LogInformation("{MethodName} {ObjectNameName}: {@technology} ", methodNameLog, nameof(item), item);

                int itemResult = await connection.ExecuteAsync(sql, item);

                if (itemResult == 0)
                    throw new Exception($"Item with id {id} not found!");

                serviceResponse.Data = itemResult;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error ocurred in {MethodName}: {Message}", $"{GetType().Name} -> {MethodBase.GetCurrentMethod()?.Name}", ex.Message);

            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return serviceResponse;
    }
}
