using Microsoft.EntityFrameworkCore;
using POS_API.DTO;




public class DataContext : DbContext
{






    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<Product> Product { get; set; }
   
}

