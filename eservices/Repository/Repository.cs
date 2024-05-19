using Microsoft.EntityFrameworkCore;
using Pattern_of_life.Data;
using Pattern_of_life.Repository.Interface;
using System.Linq.Expressions;

namespace Pattern_of_life.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<IQueryable<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = _dbSet.AsNoTracking();
            foreach (var includeProperty in includeProperties)
            {
                queryable = queryable.Include(includeProperty);
            }
            return queryable;
        }

        public async Task<T> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<T> GetByIdIncludingAsync(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync(entity => (int)entity.GetType().GetProperty("Id").GetValue(entity) == id);
        }

        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _dbContext.SaveChangesAsync();



            }
        }
    }


}
