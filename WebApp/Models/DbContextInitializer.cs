using System.Data.Entity;
using System.Linq;

namespace WebApp.Models
{
    public class DbContextInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            if (!context.Customers.Any())
            {
                context.Customers.AddRange(new Customer[]
                {
                    new Customer()
                    {
                        Id = 1,
                        FirstName = "Chris-Customer",
                        LastName = "Love"
                    },
                    new Customer()
                    {
                        Id = 2,
                        FirstName = "Daniel-Customer",
                        LastName = "Egan",
                    },
                    new Customer()
                    {
                        Id = 3,
                        FirstName = "Igor-Customer",
                        LastName = "Minar",
                    },
                    new Customer()
                    {
                        Id = 4,
                        FirstName = "Brad-Customer",
                        LastName = "Green",
                    },
                    new Customer()
                    {
                        Id = 5,
                        FirstName = "Misko-Customer",
                        LastName = "Hevery",
                    },
                });
            }

            base.Seed(context);
        }
    }
}