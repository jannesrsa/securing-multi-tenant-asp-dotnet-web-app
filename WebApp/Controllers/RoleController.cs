using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class RoleController : Controller
    {
        //private readonly IAsyncRepository<ApplicationUser> _applicationUserRepository;
        private readonly ApplicationUserManager _applicationUserManager;

        public RoleController(ApplicationUserManager applicationUserManager)
        {
            _applicationUserManager = applicationUserManager;
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> AssignUserToRole(string email, string role)
        {
            var user = await _applicationUserManager.FindByEmailAsync(email);
            await _applicationUserManager.AddToRoleAsync(user.Id, role);
            return View("AssignUserToManager", "", email + ":" + role);
        }

        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
    }
}