using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


    public class DataContext : DbContext
    {

      

     
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Product> Product { get; set; }

        public DbSet<Transaction> Transaction_ { get; set; }

        public DbSet<TransactionDetail> TransactionDetail { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:vvmdb.database.windows.net,1433;Initial Catalog=VVMAPITEST;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=Active Directory Default;");
                // Replace 'UseSqlServer' with the database provider you are using (e.g., UseSqlite, UseNpgsql for PostgreSQL, etc.)
            }
        }
    }

