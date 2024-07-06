using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using POS_API.Models;


public class DataContext : DbContext
    {





    
    public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Product> Product { get; set; }

        public DbSet<Transaction> Transaction_ { get; set; }

        public DbSet<TransactionDetail> TransactionDetail { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

        
    }

