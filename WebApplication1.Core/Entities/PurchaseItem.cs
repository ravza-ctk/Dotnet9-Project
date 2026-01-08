namespace WebApplication1.Core.Entities;

public class PurchaseItem : BaseEntity
{
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    // Foreign Key
    public int PurchaseId { get; set; }
    public Purchase Purchase { get; set; }

    public int MerchandiseId { get; set; }
    public Merchandise Merchandise { get; set; }
}
