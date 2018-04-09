using Microsoft.EntityFrameworkCore;
 
namespace bankaccount.Models
{
    public class BankContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public BankContext(DbContextOptions<BankContext> options) : base(options) { }

        // This DbSet contains objects and database table
        // DbSet<ObjectName> DatabaseTabaleName {get; set;}
        public DbSet<User> Users { get; set; }
        public DbSet<AccountTransaction> AccountTransactions { get; set; }
        
    }
}