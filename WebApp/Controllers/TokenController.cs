using System.Web.Mvc;
using WebApp.Helpers;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class TokenController : Controller
    {
        public ActionResult GetToken(string userName)
        {
            var tokenValue = JwtTokenHelper.GenerateToken(userName, out string plainToken);

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
    }
}