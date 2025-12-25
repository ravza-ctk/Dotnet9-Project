namespace WebApplication1.Core.DTOs;

public class MerchandiseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string CollectionName { get; set; } // Flattened
}

public class MerchandiseCreateDto
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int CollectionId { get; set; }
}

public class MerchandiseUpdateDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int CollectionId { get; set; }
}
