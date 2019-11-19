using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class SpeakersWebApiController : ApiController
    {
        private readonly IAsyncRepository<Speaker> _speakerRepository;
        private readonly ITenantService _tenantService;

        public SpeakersWebApiController(IAsyncRepository<Speaker> speakerRepository, ITenantService tenantService)
        {
            _speakerRepository = speakerRepository;
            _tenantService = tenantService;
        }

        // GET: api/Default
        [Authorize]
        public async Task<IEnumerable<Speaker>> Get()
        {
            var tenant = await _tenantService.GetTenantAsync();

            var speakers = await _speakerRepository.ListAllAsync();

            return speakers
                .Where(i => i.TenantId == tenant.Id);
        }

        // GET: api/Default/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Default
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Default/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Default/5
        public void Delete(int id)
        {
        }
    }
}
