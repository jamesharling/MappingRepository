using MappingRepository.Interfaces;
using MappingRepository.Tests.Mocks.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace MappingRepository.Tests.Mocks
{
    internal class DbContext : System.Data.Entity.DbContext, IMappingRepositoryDbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public override int SaveChanges()
        {
            return 1;
        }

        public override DbSet<TEntity> Set<TEntity>()
        {
            return base.Set<TEntity>();
        }

        public DbContext()
        {
            var customers = new List<Customer>();

            customers.Add(new Customer()
            {
                Id = Guid.Parse("9adf4ddf-8010-466d-b92f-df3b180b722f"),
                FirstName = "Barry",
                Surname = "Jenkins"
            });

            customers.Add(new Customer()
            {
                Id = Guid.Parse("407f8c3e-1229-466d-9e53-dd769fcc43b7"),
                FirstName = "Sarah",
                Surname = "Barnes"
            });

            this.Customers.AddRange(customers);

            var orders = new List<Order>();

            orders.Add(new Order(){
                Id = Guid.NewGuid(),
                CustomerId = Guid.Parse("9adf4ddf-8010-466d-b92f-df3b180b722f"),
                Number = 10843,
                Item = "Wind in the Willows"
            });

            orders.Add(new Order()
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.Parse("9adf4ddf-8010-466d-b92f-df3b180b722f"),
                Number = 75423,
                Item = "War and Peace"
            });

            orders.Add(new Order()
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.Parse("407f8c3e-1229-466d-9e53-dd769fcc43b7"),
                Number = 34234,
                Item = "Of Mice and Men"
            });

            orders.Add(new Order()
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.Parse("407f8c3e-1229-466d-9e53-dd769fcc43b7"),
                Number = 13243,
                Item = "1984"
            });

            orders.Add(new Order()
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.Parse("407f8c3e-1229-466d-9e53-dd769fcc43b7"),
                Number = 78955,
                Item = "Romeo and Juliet"
            });

            this.Orders.AddRange(orders);
        }
    }
}
