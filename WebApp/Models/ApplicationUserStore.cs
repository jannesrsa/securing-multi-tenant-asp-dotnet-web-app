using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class ApplicationUserStore<TUser> : UserStore<TUser>
        where TUser : ApplicationUser
    {
        public ApplicationUserStore(DbContext context) :
            base(context)
        {
        }

        public int TenantId { get; set; }

        public override Task CreateAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.TenantId = this.TenantId;

            return base.CreateAsync(user);
        }

        public override Task<TUser> FindByEmailAsync(string email)
        {
            return GetUserAggregateAsync(i => i.TenantId == this.TenantId
                                            && i.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
        }

        public override Task<TUser> FindByNameAsync(string userName)
        {
            return GetUserAggregateAsync(i => i.TenantId == this.TenantId
                                             && i.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}