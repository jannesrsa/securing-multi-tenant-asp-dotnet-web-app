using System;
using System.Data.Entity;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Data
{
    public class TenantRepository : EfRepository<Tenant>, ITenantRepository
    {
        public TenantRepository(ApplicationDbContext dbContext) :
            base(dbContext)
        {
        }

        public async Task<Tenant> GetByUrlAsync(string url)
        {
            return await _dbContext.Set<Tenant>()
                .FirstOrDefaultAsync(i => i.DomainName.Equals(url, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<Tenant> GetDefaultAsync()
        {
            return await _dbContext.Set<Tenant>()
                .FirstOrDefaultAsync(i => i.Default);
        }
    }
}