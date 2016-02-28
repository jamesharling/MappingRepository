using MappingRepository.Tests.Mocks;
using System.Data.Entity;

namespace MappingRepository.Tests.Implementations.Context
{
    public class DbInitializer : DropCreateDatabaseAlways<DbContext>
    {
        protected override void Seed(DbContext context)
        {
            context.Customers.AddRange(Data.Customers);
            context.Orders.AddRange(Data.Orders);
        }
    }
}
