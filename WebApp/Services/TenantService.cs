using System;
using System.Threading.Tasks;
using System.Web;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Services
{
    public class TenantService : ITenantService
    {
        private readonly HttpContextBase _httpContext;
        private readonly ITenantRepository _tenantRepository;

        public TenantService(ITenantRepository tenantRepository, HttpContextBase httpContext)
        {
            _tenantRepository = tenantRepository;
            _httpContext = httpContext;
        }

        public async Task<Tenant> GetTenantAsync()
        {
            Tenant tenant = await _tenantRepository.GetByUrlAsync(_httpContext.Request.Url.Host);
            if (tenant == null)
            {
                tenant = await _tenantRepository.GetDefaultAsync();

                if (tenant == null)
                {
                    throw new ApplicationException("tenant not found");
                }
            }

            return tenant;
        }
    }
}