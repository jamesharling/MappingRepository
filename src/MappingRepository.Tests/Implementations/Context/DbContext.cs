using MappingRepository.Interfaces;
using MappingRepository.Tests.Implementations.Entities;
using System.Data.Entity;

namespace MappingRepository.Tests.Implementations.Context
{
    public class DbContext : System.Data.Entity.DbContext
    {
        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Order> Orders { get; set; }
    }
}
