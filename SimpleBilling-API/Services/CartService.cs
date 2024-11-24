using SimpleBilling_API.Interfaces;
using SimpleBilling_API.Models;
using SimpleBilling_API.Models.Enums;

namespace SimpleBilling_API.Services;

public class CartService : ICartService
{
    public Bill IssueBill(ICollection<(Item dbItem, int quantity)> items, Currency currency)
    {
        Bill bill = new Bill();
        foreach ((Item dbItem, int quantity) item in items)
        {
            decimal afterDiscountPrice = CalculateAfterDiscountPrice(item.dbItem.Price, item.dbItem.Discount);
            decimal totalPrice = CalculateTotalPrice(item.quantity, item.dbItem.Price, item.dbItem.Discount);

            bill.BillItems.Add(new BillItem()
            {
                Name = item.dbItem.Name,
                Quantity = item.quantity,
                Price = item.dbItem.Price,
                Discount = item.dbItem.Discount,
                AfterDiscountPrice = afterDiscountPrice,
                TotalPrice = totalPrice
            });

            bill.SubTotal += bill.BillItems.Last().TotalPrice;
        }

        bill.Currency = currency;
        bill.Total = CalculateTotal(bill);

        return bill;
    }

    private decimal CalculateAfterDiscountPrice(decimal price, byte discount)
        => price - (price * discount / 100);

    private decimal CalculateTotalPrice(int quantity, decimal price, byte discount)
        => quantity * (price - (price * discount / 100));

    private decimal CalculateTotal(Bill bill)
        => bill.Total + (bill.SubTotal * bill.Vat / 100);
}
