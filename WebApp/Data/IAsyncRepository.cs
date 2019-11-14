using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Data
{
    public interface IAsyncRepository<T>
    {
        Task<T> AddAsync(T entity);

        void Delete(T entity);

        Task<bool> Exists(int id);

        Task<T> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> ListAllAsync();

        Task<bool> SaveAsync();

        void Update(T entity);
    }
}