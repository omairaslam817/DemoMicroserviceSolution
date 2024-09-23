using CustomerWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace CustomerWebApi.DbContexts
{
    public class CustomerDbContext:DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options):base(options) 
        {
            try
            {
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (databaseCreator != null)
                {
                    if (!databaseCreator.CanConnect())
                    {
                        databaseCreator.Create();
                    }
                    if (!databaseCreator.HasTables())
                    {
                        databaseCreator.CreateTables();
                    }

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error Ocurred in Db Context: {ex.Message}");
            }
        }
        public DbSet<Customer> Customers { get; set; }

    }
}
