using AutoMapper;
using System.Threading.Tasks;
using WebApplication1.Core.DTOs;
using WebApplication1.Core.Entities;
using WebApplication1.Core.Interfaces;
using WebApplication1.Core.Models;

namespace WebApplication1.Service.Services;

public class MerchandiseService : GenericService<Merchandise, MerchandiseDto>, IMerchandiseService
{
    public MerchandiseService(IGenericRepository<Merchandise> repository, IMapper mapper) : base(repository, mapper)
    {
    }

    public async Task<ServiceResponse<MerchandiseDto>> CreateAsync(MerchandiseCreateDto dto)
    {
        var entity = _mapper.Map<Merchandise>(dto);
        await _repository.AddAsync(entity);
        var merchandiseDto = _mapper.Map<MerchandiseDto>(entity);
        
        // Populate category name if needed, but generic map might miss it unless eager loaded. 
        // For now sufficient.
        
        return new ServiceResponse<MerchandiseDto> { Data = merchandiseDto };
    }
}
