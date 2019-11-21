using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp
{
    public class ApplicationUserStore : UserStore<ApplicationUser>
    {
        public ApplicationUserStore(DbContext context) :
            base(context)
        {
        }

        public int TenantId { get; set; }

        public override Task CreateAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.TenantId = this.TenantId;

            return base.CreateAsync(user);
        }

        public override Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return GetUserAggregateAsync(i => i.TenantId == this.TenantId
                                            && i.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
        }

        public override Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return GetUserAggregateAsync(i => i.TenantId == this.TenantId
                                             && i.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}