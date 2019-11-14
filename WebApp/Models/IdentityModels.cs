using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public int TenantId { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Tenant> Tenants { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var user = modelBuilder.Entity<ApplicationUser>();

            user.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index",
                new IndexAnnotation(new IndexAttribute("TenantUserNameIndex")
                {
                    IsUnique = true,
                    Order = 1
                }));

            user.Property(u => u.TenantId)
                .IsRequired()
                .HasColumnAnnotation("Index",
                new IndexAnnotation(new IndexAttribute("TenantUserNameIndex")
                {
                    IsUnique = true,
                    Order = 2
                }));
        }

        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry,
             IDictionary<object, object> items)
        {
            if (entityEntry != null && entityEntry.State == EntityState.Added)
            {
                var errors = new List<DbValidationError>();
                var user = entityEntry.Entity as ApplicationUser;

                //check for uniqueness of user name and email
                if (user != null)
                {
                    if (Users.Any(u => string.Equals(u.UserName, user.UserName) && user.TenantId == u.TenantId))
                    {
                        errors.Add(new DbValidationError("User",
                            string.Format(CultureInfo.CurrentCulture, IdentityResources.DuplicateUserName, user.UserName)));
                    }
                    if (RequireUniqueEmail && Users.Any(u => string.Equals(u.Email, user.Email) && user.TenantId == u.TenantId))
                    {
                        errors.Add(new DbValidationError("User",
                            string.Format(CultureInfo.CurrentCulture, IdentityResources.DuplicateEmail, user.Email)));
                    }
                }
                else
                {
                    var role = entityEntry.Entity as IdentityRole;

                    //check for uniqueness of role name
                    if (role != null && Roles.Any(r => string.Equals(r.Name, role.Name)))// && r.TenantId == r.TenantId))
                    {
                        errors.Add(new DbValidationError("Role",
                            string.Format(CultureInfo.CurrentCulture, IdentityResources.RoleAlreadyExists, role.Name)));
                    }
                }

                if (errors.Any())
                {
                    return new DbEntityValidationResult(entityEntry, errors);
                }
            }
            return base.ValidateEntity(entityEntry, items);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    [Table("ASPNETTenants")]
    public class Tenant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DomainName { get; set; }
        public bool Default { get; set; }
    }
}