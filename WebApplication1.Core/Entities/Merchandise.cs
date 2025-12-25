namespace WebApplication1.Core.Entities;

public class Merchandise : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    
    // Foreign Key
    public int CollectionId { get; set; }
    public Collection Collection { get; set; }
}
