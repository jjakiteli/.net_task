using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Backend.Db
{
    public class MyDbContext : DbContext
    {
        public MyDbContext() : base("name=MyDbConnection")
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Currency> Currencies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>()
                .HasRequired(v => v.Currency)
                .WithMany(c => c.Vehicles)
                .HasForeignKey(v => v.CurrencyId)
                .WillCascadeOnDelete(false);
        }
    }
}