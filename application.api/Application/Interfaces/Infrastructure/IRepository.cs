using System.Linq.Expressions;

namespace Application.Interfaces.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<T> FindFirstAsync(Expression<Func<T, bool>> predicate);
    }
}
