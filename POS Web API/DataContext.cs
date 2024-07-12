﻿using Microsoft.EntityFrameworkCore;




public class DataContext : DbContext
{






    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<Product> Product { get; set; }

    public DbSet<Transaction> Transaction_ { get; set; }

    public DbSet<TransactionDetail> TransactionDetail { get; set; }


}

