using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class RoleController : Controller
    {
        private readonly ApplicationUserManager _userManager;

        public RoleController(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        public RoleController()
        {
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> AssignUserToRole(string email, string role)
        {
            var user = await UserManager.FindByEmailAsync(email);
            await UserManager.AddToRoleAsync(user.Id, role);
            return View("AssignUserToManager", "", email + ":" + role);
        }

        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
    }
}