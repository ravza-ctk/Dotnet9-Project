using System.Collections.Generic;

namespace WebApplication1.Core.Entities;

public class Collection : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    // Relations
    public ICollection<Merchandise> Merchandises { get; set; } = new List<Merchandise>();
}
