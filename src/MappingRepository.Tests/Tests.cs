using AutoMapper;
using FluentAssertions;
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
        public void CanAdd()
        {
            var repo = this.getRepo();

            var customerId = Guid.Parse("87a771b8-fa67-461b-8975-e241b51dc257");

            var customer = new Implementations.DomainObjects.Customer()
            {
                Id = customerId,
                Name = "Amanda Clement"
            };

            var returnId = repo.Add(customer);

            returnId.Should().Be("87a771b8-fa67-461b-8975-e241b51dc257");

            var result = repo.GetById(Guid.Parse("87a771b8-fa67-461b-8975-e241b51dc257"));

            result.Id.Should().Be("87a771b8-fa67-461b-8975-e241b51dc257");
            result.Name.Should().Be("Amanda Clement");
        }

        [TestMethod]
        public void CanAddRange()
        {
            var repo = this.getRepo();

            var customers = new List<Implementations.DomainObjects.Customer>();

            var customer1 = new Implementations.DomainObjects.Customer()
            {
                Id = Guid.NewGuid(),
                Name
            = "Amanda Clement"
            };

            var customer2 = new Implementations.DomainObjects.Customer()
            {
                Id = Guid.NewGuid(),
                Name = "Kevin Ronalds"
            };

            customers.Add(customer1);
            customers.Add(customer2);

            repo.AddRange(customers);

            var result = repo.GetAll();

            result.Count.Should().Be(4);
        }

        [TestMethod]
        public void CanAny()
        {
            var repo = this.getRepo();

            var result = repo.Any(x => x.FirstName == "Barry");

            result.Should().Be(true);
        }

        [TestMethod]
        public void CanAsFilteredQueryable()
        {
            var repo = this.getRepo();

            var result = repo.FilteredQueryable();

            result.Count.Should().Be(1);
        }

        [TestMethod]
        public void CanAsQueryable()
        {
            var repo = this.getRepo();

            var result = repo.Queryable();

            result.Count.Should().Be(2);
        }

        //[TestMethod]
        public void CanDelete()
        {
            var repo = this.getRepo();

            var result = repo.Delete(Guid.Parse("407f8c3e-1229-466d-9e53-dd769fcc43b7"));

            result.Should().Be(1);
        }

        [TestMethod]
        public void CanDeleteRange()
        {
            var repo = this.getRepo();

            var result = repo.DeleteRange(x => x.FirstName != null);

            result.Should().Be(2);
        }

        [TestMethod]
        public void CanEdit()
        {
            var repo = this.getRepo();

            var customer = repo.GetById(Guid.Parse("407f8c3e-1229-466d-9e53-dd769fcc43b7"));

            customer.Name = "Sarah Moore";

            var result = repo.Edit(customer);

            //result.Should().Be(1);

            customer = repo.GetById(Guid.Parse("407f8c3e-1229-466d-9e53-dd769fcc43b7"));

            customer.Name.Should().Be("Sarah Moore");
        }

        [TestMethod]
        public void CanFindBy()
        {
            var repo = this.getRepo();

            var result = repo.FindBy(x => x.FirstName.Equals("Barry"));

            result.Count.Should().Be(1);
            result.Single().Name.Should().Be("Barry Jenkins");
        }

        [TestMethod]
        public void CanFirstOrDefault()
        {
            var repo = this.getRepo();

            var result = repo.FirstOrDefault(x => x.FirstName.Equals("Barry"));

            result.Name.Should().Be("Barry Jenkins");

            result = repo.FirstOrDefault(x => x.FirstName.Equals("Jackie"));

            result.Should().Be(null);
        }

        [TestMethod]
        public void CanGetById()
        {
            var repo = this.getRepo();

            var result = repo.GetById(Guid.Parse("407f8c3e-1229-466d-9e53-dd769fcc43b7"));

            result.Name.Should().Be("Sarah Barnes");
        }

        [TestMethod]
        public void CanProjectTo()
        {
            var repo = this.getRepo();

            var result = repo.GetAllAsLite();

            result.Count.Should().Be(2);
            result.First().GetType().Should().Be(typeof(Implementations.DomainObjects.LiteCustomer));
        }

        [TestMethod]
        public void CanSingleOrDefault()
        {
            var repo = this.getRepo();

            var result = repo.SingleOrDefault(x => x.FirstName.Equals("Barry"));

            result.Name.Should().Be("Barry Jenkins");

            try
            {
                result = repo.SingleOrDefault(x => x.FirstName != null);
            }
            catch (Exception ex)
            {
                ex.GetType().Should().Be(typeof(InvalidOperationException));
            }
        }

        private IMapper mapper
        {
            get
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AllowNullCollections = true;

                    cfg.CreateMap<Implementations.Entities.Customer, Implementations.DomainObjects.Customer>();
                    cfg.CreateMap<Implementations.Entities.Customer, Implementations.DomainObjects.LiteCustomer>();
                    cfg.CreateMap<Implementations.Entities.Order, Implementations.DomainObjects.Order>();

                    cfg.CreateMap<Implementations.DomainObjects.Customer, Implementations.Entities.Customer>()
                        //.ForMember(d => d.Id, o => o.Ignore())
                        .ForMember(d => d.FirstName, o => o.MapFrom(s => s.Name.Split(' ')[0]))
                        .ForMember(d => d.Surname, o => o.MapFrom(s => s.Name.Split(' ')[1]));

                    cfg.CreateMap<Implementations.DomainObjects.Order, Implementations.Entities.Order>();
                        //.ForMember(d => d.Id, o => o.Ignore());
                });

                return config.CreateMapper();
            }
        }

        private CustomerRepository getRepo() => new CustomerRepository(EFMockHelpers.GetMockedContext().Object, this.mapper);
    }
}
