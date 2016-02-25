using AutoMapper;
using MappingRepository.Interfaces;
using MappingRepository.Tests.Mocks;
using MappingRepository.Tests.Mocks.Entities;
using MappingRepository.Tests.Mocks.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data.Entity;
using System.Linq;

namespace MappingRepository.Tests
{
    [TestClass]
    public class Tests
    {
        private static IMapper mapper
        {
            get
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Mocks.Entities.Customer, Mocks.DomainObjects.Customer>();
                    cfg.CreateMap<Mocks.Entities.Order, Mocks.DomainObjects.Order>();
                });

                return config.CreateMapper();
            }
        }

        private static Mock<Mocks.Context.DbContext> mockContext
        {
            get
            {   
                var mockDbSetCustomers = new Mock<DbSet<Customer>>();
                mockDbSetCustomers.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(Data.Customers.Provider);
                mockDbSetCustomers.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(Data.Customers.Expression);
                mockDbSetCustomers.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(Data.Customers.ElementType);
                mockDbSetCustomers.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(Data.Customers.GetEnumerator());

                var mockDbSetOrders = new Mock<DbSet<Order>>();
                mockDbSetOrders.As<IQueryable<Order>>().Setup(m => m.Provider).Returns(Data.Orders.Provider);
                mockDbSetOrders.As<IQueryable<Order>>().Setup(m => m.Expression).Returns(Data.Orders.Expression);
                mockDbSetOrders.As<IQueryable<Order>>().Setup(m => m.ElementType).Returns(Data.Orders.ElementType);
                mockDbSetOrders.As<IQueryable<Order>>().Setup(m => m.GetEnumerator()).Returns(Data.Orders.GetEnumerator());

                var mockContext = new Mock<Mocks.Context.DbContext>();

                mockContext.Setup(m => m.Customers).Returns(mockDbSetCustomers.Object);
                mockContext.Setup(m => m.Set<Customer>()).Returns(mockDbSetCustomers.Object);
                mockContext.Setup(m => m.Orders).Returns(mockDbSetOrders.Object);
                mockContext.Setup(m => m.Set<Order>()).Returns(mockDbSetOrders.Object);

                return mockContext;
            }
        }

        [TestMethod]
        public void CanGetById()
        {
            var repo = new CustomerRepository(mockContext.Object, mapper);

            var customerId = Guid.Parse("9adf4ddf-8010-466d-b92f-df3b180b722f");

            var customer = repo.GetById(customerId);

        }
    }
}
