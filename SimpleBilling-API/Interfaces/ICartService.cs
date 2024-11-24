using SimpleBilling_API.Models;
using SimpleBilling_API.Models.Enums;

namespace SimpleBilling_API.Interfaces;

public interface ICartService
{
    Bill IssueBill(ICollection<(Item dbItem, int quantity)> items, Currency currency);
}
