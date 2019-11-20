namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebApp.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebApp.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            if (!context.Tenants.Any())
            {
                context.Tenants.AddRange(new Tenant[]
                {
                    new Tenant()
                    {
                        Id = 1,
                        Name = "SVCC",
                        Default = true,
                        DomainName = "siliconvalley-codecamp.com",
                    },
                    new Tenant()
                    {
                        Id = 2,
                        Name = "ANGU",
                        Default = false,
                        DomainName = "angular.com",
                    },
                });
            }

            if (!context.Speakers.Any())
            {
                context.Speakers.AddRange(new Speaker[]
                {
                    new Speaker()
                    {
                        Id = 1,
                        First = "Chris",
                        Last = "Love",
                        TenantId = 1,
                    },
                    new Speaker()
                    {
                        Id = 2,
                        First = "Daniel",
                        Last = "Egan",
                        TenantId = 1,
                    },
                    new Speaker()
                    {
                        Id = 3,
                        First = "Igor",
                        Last = "Minar",
                        TenantId = 2,
                    },
                    new Speaker()
                    {
                        Id = 4,
                        First = "Brad",
                        Last = "Green",
                        TenantId = 2,
                    },
                    new Speaker()
                    {
                        Id = 5,
                        First = "Misko",
                        Last = "Hevery",
                        TenantId = 2,
                    },
                });
            }

            if (!context.Customers.Any())
            {
                context.Customers.AddRange(new Customer[]
                {
                    new Customer()
                    {
                        Id = 1,
                        FirstName = "Customer - Chris",
                        LastName = "Customer - Love",
                    },
                    new Customer()
                    {
                        Id = 2,
                        FirstName = "Customer - Daniel",
                        LastName = "Customer - Egan",
                    },
                    new Customer()
                    {
                        Id = 3,
                        FirstName = "Customer - Igor",
                        LastName = "Customer - Minar",
                    },
                    new Customer()
                    {
                        Id = 4,
                        FirstName = "Customer - Brad",
                        LastName = "Customer - Green",
                    },
                    new Customer()
                    {
                        Id = 5,
                        FirstName = "Customer - Misko",
                        LastName = "Customer - Hevery",
                    },
                });
            }

            base.Seed(context);
        }
    }
}
