namespace WebApp.Data
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using WebApp.Models;

    public class EfRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        protected readonly DbContext _dbContext;

        public EfRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            var addedEntity = _dbContext.Set<T>().Add(entity);
            return await Task.FromResult(addedEntity);
        }

        public virtual void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public virtual async Task<bool> Exists(int id)
        {
            return await _dbContext.Set<T>().AnyAsync(i => i.Id == id);
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public virtual async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public virtual async Task<bool> SaveAsync()
        {
            var numberOfStateEntriesWrittenToDatabase = await _dbContext.SaveChangesAsync();
            return numberOfStateEntriesWrittenToDatabase > 0;
        }

        public virtual void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}