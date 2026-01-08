using System.Threading.Tasks;
using WebApplication1.Core.DTOs;
using WebApplication1.Core.Entities;
using WebApplication1.Core.Models;

namespace WebApplication1.Core.Interfaces;

public interface IMerchandiseService : IGenericService<Merchandise, MerchandiseDto>
{
    Task<ServiceResponse<MerchandiseDto>> CreateAsync(MerchandiseCreateDto dto);
}
