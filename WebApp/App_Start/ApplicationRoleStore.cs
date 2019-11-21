using Microsoft.AspNet.Identity.EntityFramework;
using WebApp.Models;

namespace WebApp
{
    public class ApplicationRoleStore : RoleStore<IdentityRole>
    {
        public ApplicationRoleStore(ApplicationDbContext context) : base(context)
        {
        }
    }
}