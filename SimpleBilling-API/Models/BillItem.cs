namespace SimpleBilling_API.Models;

public class BillItem
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public byte Discount { get; set; }
    public decimal AfterDiscountPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}
