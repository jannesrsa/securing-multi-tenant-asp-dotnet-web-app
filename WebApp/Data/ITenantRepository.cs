using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Data
{
    public interface ITenantRepository : IAsyncRepository<Tenant>
    {
        Task<Tenant> GetByUrlAsync(string url);

        Task<Tenant> GetDefaultAsync();
    }
}