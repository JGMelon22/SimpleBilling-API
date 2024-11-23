using System.Text.Json.Serialization;
using SimpleBilling_API.Models.Enums;

namespace SimpleBilling_API.Models;

public class Bill
{
    public ICollection<BillItem> BillItems { get; set; }
    public decimal SubTotal { get; set; }

    [JsonPropertyName("VAT")]
    public int Vat { get; set; }
    public Currency Currency { get; set; }
    public decimal Total { get; set; }

    public Bill()
    {
        BillItems = new List<BillItem>();
        Vat = 20;
        SubTotal = Total = 0;
    }
}
