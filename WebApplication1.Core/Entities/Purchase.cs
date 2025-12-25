using System;
using System.Collections.Generic;

namespace WebApplication1.Core.Entities;

public class Purchase : BaseEntity
{
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }

    // Foreign Key
    public int UserId { get; set; }
    public User User { get; set; }

    // Relations
    public ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();
}
