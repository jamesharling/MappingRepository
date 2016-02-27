using MappingRepository.Tests.Implementations.Entities;
using System;
using System.Collections.Generic;

namespace MappingRepository.Tests.Mocks
{
    internal static class Data
    {
        internal static List<Customer> Customers
        {
            get
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

                return customers;
            }
        }

        internal static List<Order> Orders
        {
            get
            {
                var orders = new List<Order>();

                orders.Add(new Order()
                {
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

                return orders;
            }
        }
    }
}
