using AutoMapper;
using MappingRepository.Tests.Implementations.Context;
using MappingRepository.Tests.Implementations.Entities;
using MappingRepository.Tests.Implementations.Repositories;
using MappingRepository.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MappingRepository.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void CanGetById()
        {
            var mockContext = this.GetNewMockContext();

            var repo = this.GetNewRepository(mockContext);

            var customerId = Guid.Parse("9adf4ddf-8010-466d-b92f-df3b180b722f");

            var customer = repo.GetById(customerId);

            Assert.AreEqual(customerId, customer.Id);
        }

        [TestMethod]
        public void CanAdd()
        {
            var mockContext = this.GetNewMockContext();

            var mockDbSetCustomer = EFMockHelper.MockDbSet(Data.Customers);

            this.SetupMockedDbContext(mockContext, mockDbSetCustomer);

            var repo = this.GetNewRepository(mockContext);

            var customerId = Guid.Parse("6ff46201-7755-498a-a6e4-28d65f2ce0bb");

            var customer = new Implementations.DomainObjects.Customer()
            {
                Id = customerId,
                Name = "Amanda Clement"
            };

            var result = repo.Add(customer);

            mockDbSetCustomer.Verify(m => m.Add(It.IsAny<Customer>()), Times.Once);
            mockContext.Verify(x => x.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void CanAddRange()
        {
            var mockContext = this.GetNewMockContext();

            var mockDbSetCustomer = EFMockHelper.MockDbSet(Data.Customers);

            this.SetupMockedDbContext(mockContext, mockDbSetCustomer);

            var repo = this.GetNewRepository(mockContext);

            var customers = new List<Implementations.DomainObjects.Customer>();

            var customer1 = new Implementations.DomainObjects.Customer()
            {
                Id = Guid.NewGuid(),
                Name = "Amanda Clement"
            };

            var customer2 = new Implementations.DomainObjects.Customer()
            {
                Id = Guid.NewGuid(),
                Name = "Kevin Ronalds"
            };

            customers.Add(customer1);
            customers.Add(customer2);

            var result = repo.AddRange(customers);

            mockDbSetCustomer.Verify(m => m.AddRange(It.IsAny<IEnumerable<Customer>>()), Times.Once);
            mockContext.Verify(x => x.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void CanAny()
        {
            var mockContext = this.GetNewMockContext();

            var mockDbSetCustomer = EFMockHelper.MockDbSet(Data.Customers);

            this.SetupMockedDbContext(mockContext, mockDbSetCustomer);

            var repo = this.GetNewRepository(mockContext);

            var result = repo.Any(x => x.FirstName == "Barry");

            Assert.AreEqual(true, result);
        }

        private IMapper mapper
        {
            get
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Implementations.Entities.Customer, Implementations.DomainObjects.Customer>();
                    cfg.CreateMap<Implementations.Entities.Order, Implementations.DomainObjects.Order>();

                    cfg.CreateMap<Implementations.DomainObjects.Customer, Implementations.Entities.Customer>()
                        .ForMember(d => d.FirstName, o => o.MapFrom(s => s.Name.Split(' ')[0]))
                        .ForMember(d => d.Surname, o => o.MapFrom(s => s.Name.Split(' ')[1]));
                    cfg.CreateMap<Implementations.DomainObjects.Order, Implementations.Entities.Order>();
                });

                return config.CreateMapper();
            }
        }

        private MockedDbContext<DbContext> GetNewMockContext()
        {
            return EFMockHelper.GetMockContext<DbContext>();
        }

        private void SetupMockedDbContext<T>(MockedDbContext<DbContext> mockContext, params Mock<System.Data.Entity.DbSet<T>>[] dbSets) where T : class
        {
            foreach (var dbSet in dbSets)
                mockContext.Setup(m => m.Set<T>()).Returns(dbSet.Object);
        }

        private CustomerRepository GetNewRepository(MockedDbContext<DbContext> mockContext)
        {
            return new CustomerRepository(mockContext.Object, mapper);
        }
    }
}
