using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Core.Entities;
using WebApplication1.Core.Interfaces;
using WebApplication1.Core.Models;

namespace WebApplication1.Service.Services;

public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto> 
    where TEntity : BaseEntity 
    where TDto : class
{
    protected readonly IGenericRepository<TEntity> _repository;
    protected readonly IMapper _mapper;

    public GenericService(IGenericRepository<TEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public virtual async Task<ServiceResponse<IEnumerable<TDto>>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
        return new ServiceResponse<IEnumerable<TDto>> { Data = dtos };
    }

    public virtual async Task<ServiceResponse<TDto>> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
        {
             return new ServiceResponse<TDto> { Success = false, Message = "Not Found" };
        }
        var dto = _mapper.Map<TDto>(entity);
        return new ServiceResponse<TDto> { Data = dto };
    }

    public async Task<ServiceResponse<TDto>> AddAsync(TDto dto)
    {
        var entity = _mapper.Map<TEntity>(dto);
        await _repository.AddAsync(entity);
        var newDto = _mapper.Map<TDto>(entity);
        return new ServiceResponse<TDto> { Data = newDto };
    }

    public async Task<ServiceResponse<bool>> UpdateAsync(int id, TDto dto)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
             return new ServiceResponse<bool> { Success = false, Message = "Not Found" };

        _mapper.Map(dto, entity);
        entity.UpdatedAt = System.DateTime.UtcNow;
        await _repository.UpdateAsync(entity);
        return new ServiceResponse<bool> { Data = true };
    }

    public async Task<ServiceResponse<bool>> DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
             return new ServiceResponse<bool> { Success = false, Message = "Not Found" };

        await _repository.SoftDeleteAsync(entity); // bonus olarak soft dejete kullandim
        return new ServiceResponse<bool> { Data = true };
    }
}
