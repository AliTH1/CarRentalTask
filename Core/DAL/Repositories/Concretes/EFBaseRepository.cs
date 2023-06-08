using CarRental.Core.DAL.Repositories.Abstracts;
using CarRental.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CarRental.Core.DAL.Repositories.Concretes
{
    public class EFBaseRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class, new()
        where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly DbSet<TEntity> _dbset;
        public EFBaseRepository(TContext context)
        {
            _context = context;
            _dbset = _context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbset.AddAsync(entity);
            await SaveAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _dbset.Remove(entity);
            await SaveAsync();
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> filter, params string[] includes)
        {
            IQueryable<TEntity> query = GetQuery(includes);
            return await query.Where(filter).FirstOrDefaultAsync();

        }

        public async Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter = null, params string[] includes)
        {
            IQueryable<TEntity> query = GetQuery(includes);
            return await (filter == null
                ? query.ToListAsync()
                : query.Where(filter).ToListAsync());
        }

        public async Task<List<TEntity>> GetAllPaginated(int page, int size, Expression<Func<TEntity, bool>> filter = null
            , params string[] includes)
        {
            IQueryable<TEntity> query = GetQuery(includes);

            return await (
                filter == null
                ? query.Skip((page - 1) * size).Take(size).ToListAsync()
                : query.Where(filter).Skip((page - 1) * size).Take(size).ToListAsync());
        }


        public async Task UpdateAsync(TEntity entity)
        {
            _dbset.Update(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        private IQueryable<TEntity> GetQuery(string[] includes)
        {
            IQueryable<TEntity> query = _dbset;
            foreach (string include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }
    }
}
