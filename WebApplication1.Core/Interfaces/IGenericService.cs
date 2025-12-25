using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication1.Core.Models;

namespace WebApplication1.Core.Interfaces;

public interface IGenericService<TEntity, TDto> where TEntity : class where TDto : class
{
    Task<ServiceResponse<IEnumerable<TDto>>> GetAllAsync();
    Task<ServiceResponse<TDto>> GetByIdAsync(int id);
    Task<ServiceResponse<TDto>> AddAsync(TDto dto);
    Task<ServiceResponse<bool>> UpdateAsync(int id, TDto dto); // Or return TDto
    Task<ServiceResponse<bool>> DeleteAsync(int id);
}
