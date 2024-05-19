using Pattern_of_life.Models;
using System.Linq.Expressions;

namespace Pattern_of_life.Repository.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(int id);
        Task<IQueryable<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetByIdIncludingAsync(int id, params Expression<Func<T, object>>[] includeProperties);


    }
}
