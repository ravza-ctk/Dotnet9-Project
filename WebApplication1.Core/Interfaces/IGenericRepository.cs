using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApplication1.Core.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
    Task<T> GetByIdAsync(int id, params string[] includes); // String based for deep includes
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
    Task<IEnumerable<T>> GetAllAsync(params string[] includes); // String based
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task SoftDeleteAsync(T entity); // For bonus
}
