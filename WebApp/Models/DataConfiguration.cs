using System.Data.Entity;

namespace WebApp.Models
{
    public class DataConfiguration : DbConfiguration
    {
        public DataConfiguration()
        {
            SetDatabaseInitializer(new DbContextInitializer());
        }
    }
}