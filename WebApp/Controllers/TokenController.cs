using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.Helpers;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class TokenController : Controller
    {
        private readonly ITenantService _tenantService;

        public TokenController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        public async Task<ActionResult> GetToken(string userName)
        {
            var tenentId = await _tenantService.GetTenantAsync();

            var tokenValue = JwtTokenHelper.GenerateToken(
                userName,
                out string plainToken,
                tenentId.DomainName);

            var modelData = new TokenViewModel
            {
                PlainToken = plainToken,
                TokenValue = tokenValue
            };

            return View(modelData);
        }

        public ActionResult TestToken()
        {
            var modelData = new TokenViewModel
            {
                TokenValue = ""
            };

            return View(modelData);
        }

        [HttpPost]
        public ActionResult TestToken(string tokenValue)
        {
            var principal = JwtTokenHelper.GetPrincipal(tokenValue, out string plainToken);

            var modelData = new TokenViewModel
            {
                UserName = principal.Identity.Name,
                PlainToken = plainToken,
            };

            return View("ShowUser", modelData);
        }

        [HttpPost]
        public ActionResult TestTokenFull(string tokenValue)
        {
            string plainToken;
            string tenantName;
            var username =
                JwtTokenHelper.GetPrincipalAndTenantName
                (tokenValue,
                    out plainToken,
                    out tenantName).Identity.Name;

            var modelData = new TokenViewModel
            {
                UserName = username,
                TenantName = tenantName,
                PlainToken = plainToken
            };
            return View("ShowUserFull", modelData);
        }
    }
}