using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FunctionLibrary.Models;
using Microsoft.EntityFrameworkCore;
namespace FunctionLibrary
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Product> Product { get; set; }

        public DbSet<Transaction> Transaction_ { get; set; }



    }
}
