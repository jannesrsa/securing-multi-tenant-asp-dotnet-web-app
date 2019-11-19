using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Services
{
    public interface ITenantService
    {
        Task<Tenant> GetTenantAsync();
    }
}