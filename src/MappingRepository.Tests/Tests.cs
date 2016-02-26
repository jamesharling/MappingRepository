using AutoMapper;
using FluentAssertions;
using MappingRepository.Tests.Implementations.Context;
using MappingRepository.Tests.Implementations.Entities;
using MappingRepository.Tests.Implementations.Repositories;
using MappingRepository.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;

namespace MappingRepository.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void CanGetById()
        {
            var result = repo.GetById(Guid.Parse("407f8c3e-1229-466d-9e53-dd769fcc43b7"));

            result.Name.Should().Be("Sarah Barnes");
        }

        [TestMethod]
        public void CanAdd()
        {
            var customerId = Guid.Parse("87a771b8-fa67-461b-8975-e241b51dc257");

            var customer = new Implementations.DomainObjects.Customer()
            {
                Id = customerId,
                Name = "Amanda Clement"
            };

            repo.Add(customer);

            var result = repo.GetById(Guid.Parse("87a771b8-fa67-461b-8975-e241b51dc257"));

            result.Id.Should().Be("407f8c3e-1229-466d-9e53-dd769fcc43b7");
            result.Name.Should().Be("Amanda Clement");
        }

        //[TestMethod]
        //public void CanAddRange()
        //{
        //    var mockContext = this.GetNewMockContext();

        // var mockDbSetCustomer = EFMockHelpers.MockDbSet(Data.Customers);

        // this.SetupMockedDbContext(mockContext, mockDbSetCustomer);

        // var repo = this.GetNewRepository(mockContext);

        // var customers = new List<Implementations.DomainObjects.Customer>();

        // var customer1 = new Implementations.DomainObjects.Customer() { Id = Guid.NewGuid(), Name
        // = "Amanda Clement" };

        // var customer2 = new Implementations.DomainObjects.Customer() { Id = Guid.NewGuid(), Name
        // = "Kevin Ronalds" };

        // customers.Add(customer1); customers.Add(customer2);

        // var result = repo.AddRange(customers);

        //    mockDbSetCustomer.Verify(m => m.AddRange(It.IsAny<IEnumerable<Customer>>()), Times.Once);
        //    mockContext.Verify(x => x.SaveChanges(), Times.Once());
        //}

        //[TestMethod]
        //public void CanAny()
        //{
        //    var mockContext = this.GetNewMockContext();

        // var mockDbSetCustomer = EFMockHelpers.MockDbSet(Data.Customers);

        // this.SetupMockedDbContext(mockContext, mockDbSetCustomer);

        // var repo = this.GetNewRepository(mockContext);

        // var result = repo.Any(x => x.FirstName == "Barry");

        //    Assert.AreEqual(true, result);
        //}

        [TestMethod]
        public void CanFindBy()
        {
            var result = repo.FindBy(x => x.FirstName.Equals("Barry"));

            result.Count.Should().Be(1);
            result.Single().Name.Should().Be("Barry Jenkins");
        }

        private Mock<DbContext> mockedContext
        {
            get
            {
                var mockedContext = new Mock<DbContext>();

                mockedContext.Setup(c => c.Set<Customer>()).ReturnsDbSet(Data.Customers);
                mockedContext.Setup(c => c.Set<Order>()).ReturnsDbSet(Data.Orders);

                return mockedContext;
            }
        }

        private CustomerRepository repo => new CustomerRepository(mockedContext.Object, this.mapper);

        private IMapper mapper
        {
            get
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AllowNullCollections = true;

                    cfg.CreateMap<Implementations.Entities.Customer, Implementations.DomainObjects.Customer>();
                    cfg.CreateMap<Implementations.Entities.Order, Implementations.DomainObjects.Order>();

                    cfg.CreateMap<Implementations.DomainObjects.Customer, Implementations.Entities.Customer>();
                    cfg.CreateMap<Implementations.DomainObjects.Order, Implementations.Entities.Order>();
                });

                return config.CreateMapper();
            }
        }
    }
}
