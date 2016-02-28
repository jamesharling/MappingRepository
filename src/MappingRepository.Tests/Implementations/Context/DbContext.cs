using MappingRepository.Tests.Implementations.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MappingRepository.Tests.Implementations.Context
{
    public class DbContext : System.Data.Entity.DbContext
    {
        public DbContext() : base("Default")
        {
            this.Database.Initialize(true);
        }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Properties<Guid>()
                .Where(p => p.Name == "Id")
                .Configure(p => p.IsKey().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity));
        }
    }
}
