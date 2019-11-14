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
using WebApp.Data;

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

        public DbSet<Speaker> Speakers { get; set; }

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
    public class Tenant : BaseEntity
    {
        public string Name { get; set; }
        public string DomainName { get; set; }
        public bool Default { get; set; }
    }

    [Table("ASPNETSpeakers")]
    public class Speaker : BaseEntity
    {
        public string First { get; set; }
        public string Last { get; set; }
        public int TenantId { get; set; }
    }
}

//public class ApplicationDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
//{
//    protected override void Seed(ApplicationDbContext context)
//    {
//        InitializeIdentityForEF(context);

//        base.Seed(context);
//    }

//    public static void InitializeIdentityForEF(ApplicationDbContext db)
//    {
//        if (!db.Tenants.Any())
//        {
//            db.Tenants.AddRange(new Tenant[]
//            {
//                new Tenant()
//                {
//                    Id = 1,
//                    Name = "SVCC",
//                    Default = true,
//                    DomainName = "siliconvalley-codecamp.com",
//                },
//                new Tenant()
//                {
//                    Id = 2,
//                    Name = "ANGU",
//                    Default = false,
//                    DomainName = "angular.com",
//                },
//            });
//        }

//        if (!db.Speakers.Any())
//        {
//            db.Speakers.AddRange(new Speaker[]
//            {
//                new Speaker()
//                {
//                    Id = 1,
//                    First = "Chris",
//                    Last = "Love",
//                    TenantId = 1,
//                },
//                new Speaker()
//                {
//                    Id = 2,
//                    First = "Daniel",
//                    Last = "Egan",
//                    TenantId = 1,
//                },
//                new Speaker()
//                {
//                    Id = 3,
//                    First = "Igor",
//                    Last = "Minar",
//                    TenantId = 2,
//                },
//                new Speaker()
//                {
//                    Id = 4,
//                    First = "Brad",
//                    Last = "Green",
//                    TenantId = 2,
//                },
//                new Speaker()
//                {
//                    Id = 5,
//                    First = "Misko",
//                    Last = "Hevery",
//                    TenantId = 2,
//                },
//            });
//        }

//        //var roleStore = new RoleStore<IdentityRole>(db);
//        //var roleManager = new RoleManager<IdentityRole>(roleStore);
//        //var userStore = new UserStore<ApplicationUser>(db);
//        //var userManager = new UserManager<ApplicationUser>(userStore);

//        //// Add missing roles
//        //var role = roleManager.FindByName("Admin");
//        //if (role == null)
//        //{
//        //    role = new IdentityRole("Admin");
//        //    roleManager.Create(role);
//        //}

//        //// Create test users
//        //var user = userManager.FindByName("admin");
//        //if (user == null)
//        //{
//        //    var newUser = new ApplicationUser()
//        //    {
//        //        UserName = "admin",
//        //        FirstName = "Admin",
//        //        LastName = "User",
//        //        Email = "xxx@xxx.net",
//        //        PhoneNumber = "5551234567",
//        //        MustChangePassword = false
//        //    };
//        //    userManager.Create(newUser, "Password1");
//        //    userManager.SetLockoutEnabled(newUser.Id, false);
//        //    userManager.AddToRole(newUser.Id, "Admin");
//        //}
//    }
//}