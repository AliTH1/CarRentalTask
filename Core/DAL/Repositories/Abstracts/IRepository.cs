using CarRental.Models;
using System.Linq.Expressions;

namespace CarRental.Core.DAL.Repositories.Abstracts
{
    public interface IRepository<T>
    {
        Task<T> Get(Expression<Func<T, bool>> filter, params string[] includes);
        Task<List<T>> GetAll(Expression<Func<T, bool>> filter = null, params string[] includes);

        Task<List<T>> GetAllPaginated(int page, int size,
            Expression<Func<T, bool>> filter = null, params string[] includes);

        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
