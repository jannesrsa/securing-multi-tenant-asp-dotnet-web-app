using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Mvc;
using WebApp.Models;

[assembly: OwinStartupAttribute(typeof(WebApp.Startup))]

namespace WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            CreateLocalRoles();
        }

        private void CreateLocalRoles()
        {
            ApplicationDbContext context =
                new ApplicationDbContext();
            var roleManager =
                new RoleManager<IdentityRole>
                    (new RoleStore<IdentityRole>(context));
            var userManager =
                new UserManager<ApplicationUser>
                    (new UserStore<ApplicationUser>(context));

            var roleNames = new string[]
                {"admin", "manager", "employee"};

            foreach (var roleName in roleNames)
            {
                if (roleManager.RoleExists(roleName)) continue;
                var role = new IdentityRole { Name = roleName };
                roleManager.Create(role);
            }

            var rootUser = userManager.FindByEmail("root@root.com");
            if (rootUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "root@root.com",
                    Email = "root@root.com"
                };
                string userPassword = "Abc123!";

                var chkUser = userManager.Create(user, userPassword);
                //Add default User to Role Admin
                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(user.Id, "admin");
                }
            }
        }
    }
}