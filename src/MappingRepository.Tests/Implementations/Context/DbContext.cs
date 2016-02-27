using MappingRepository.Tests.Implementations.Entities;
using MappingRepository.Tests.Mocks;
using System.Data.Entity;

namespace MappingRepository.Tests.Implementations.Context
{
    public class DbContext : System.Data.Entity.DbContext
    {
        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public override int SaveChanges()
        {
            int changes = 0;

            changes += EFMockHelpers.GenerateIDs(x => x.Id, this.Customers);
            changes += EFMockHelpers.GenerateIDs(x => x.Id, this.Orders);

            return changes;
        }
    }
}
