using System;
using System.Collections.Generic;

namespace WebApplication1.Core.DTOs;

public class PurchaseDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Username { get; set; }
    public List<PurchaseItemDto> PurchaseItems { get; set; }
}

public class PurchaseItemDto
{
    public string MerchandiseName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class CreatePurchaseDto
{
    public int UserId { get; set; }
    public List<CreatePurchaseItemDto> PurchaseItems { get; set; }
}

public class CreatePurchaseItemDto
{
    public int MerchandiseId { get; set; }
    public int Quantity { get; set; }
}
