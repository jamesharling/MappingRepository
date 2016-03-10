using AutoMapper;
using FluentAssertions;
using MappingRepository.Tests.Implementations.Context;
using MappingRepository.Tests.Implementations.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MappingRepository.Tests
{
    [TestClass]
    public class Tests
    {
        public Tests()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
            Database.SetInitializer(new DbInitializer());
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
                        .ForMember(d => d.Id, o => o.Ignore())
                        .ForMember(d => d.FirstName, o => o.MapFrom(s => s.Name.Split(' ')[0]))
                        .ForMember(d => d.Surname, o => o.MapFrom(s => s.Name.Split(' ')[1]));

                    cfg.CreateMap<Implementations.DomainObjects.Order, Implementations.Entities.Order>()
                        .ForMember(d => d.Id, o => o.Ignore());
                });

                return config.CreateMapper();
            }
        }

        private IMapper mapperThatIgnoresChildren
        {
            get
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AllowNullCollections = true;

                    cfg.CreateMap<Implementations.Entities.Customer, Implementations.DomainObjects.Customer>()
                        .ForMember(d => d.Orders, o => o.Ignore());
                    cfg.CreateMap<Implementations.Entities.Customer, Implementations.DomainObjects.LiteCustomer>();
                    cfg.CreateMap<Implementations.Entities.Order, Implementations.DomainObjects.Order>();

                    cfg.CreateMap<Implementations.DomainObjects.Customer, Implementations.Entities.Customer>()
                        .ForMember(d => d.Id, o => o.Ignore())
                        .ForMember(d => d.FirstName, o => o.MapFrom(s => s.Name.Split(' ')[0]))
                        .ForMember(d => d.Surname, o => o.MapFrom(s => s.Name.Split(' ')[1]))
                        .ForMember(d => d.Orders, o => o.Ignore());

                    cfg.CreateMap<Implementations.DomainObjects.Order, Implementations.Entities.Order>()
                        .ForMember(d => d.Id, o => o.Ignore());
                });

                return config.CreateMapper();
            }
        }

        [TestMethod]
        public void Add()
        {
            var repo = this.getRepo();

            int count = repo.Count();

            count.Should().Be(2);

            var customer = new Implementations.DomainObjects.Customer()
            {
                Name = "Amanda Clement"
            };

            var x = repo.Add(customer);

            count = repo.Count();

            count.Should().Be(3);
        }

        [TestMethod]
        public void AddRange()
        {
            var repo = this.getRepo();

            var customers = new List<Implementations.DomainObjects.Customer>();

            var customer1 = new Implementations.DomainObjects.Customer()
            {
                Name = "Amanda Clement"
            };

            var customer2 = new Implementations.DomainObjects.Customer()
            {
                Name = "Kevin Ronalds"
            };

            customers.Add(customer1);
            customers.Add(customer2);

            repo.AddRange(customers);

            var result = repo.Count();

            result.Should().Be(4);
        }

        [TestMethod]
        public void Any()
        {
            var repo = this.getRepo();

            var result = repo.Any(x => x.FirstName == "Barry");

            result.Should().Be(true);
        }

        [TestMethod]
        public void AsQueryable()
        {
            var repo = this.getRepo();

            var result = repo.Queryable();

            result.Count.Should().Be(2);
        }

        [TestMethod]
        public void Delete()
        {
            var repo = this.getRepo();

            var customerToDelete = repo.FirstOrDefault(x => x.FirstName.Equals("Barry"));

            customerToDelete.Should().NotBeNull();

            var result = repo.Delete(customerToDelete.Id);

            result.Should().Be(1);
        }

        [TestMethod]
        public void DeleteRange()
        {
            var repo = this.getRepo();

            var result = repo.DeleteRange(x => x.FirstName != null);

            result.Should().Be(2);
        }

        [TestMethod]
        public void Edit()
        {
            var repo = this.getRepo(this.mapperThatIgnoresChildren);

            var customerToEdit = repo.FirstOrDefault(x => x.FirstName.Equals("Sarah"));

            customerToEdit.Should().NotBeNull();

            customerToEdit.Name = "Sarah Moore";

            var result = repo.Edit(customerToEdit);

            result.Should().Be(1);

            var customer = repo.GetById(customerToEdit.Id);

            customer.Name.Should().Be("Sarah Moore");
        }

        [TestMethod]
        public void EditWithChildObjects()
        {
            var repo = this.getRepo();

            var customerToEdit = repo.FirstOrDefault(x => x.FirstName.Equals("Sarah"));

            customerToEdit.Should().NotBeNull();

            customerToEdit.Name = "Sarah Moore";
            customerToEdit.Orders = new List<Implementations.DomainObjects.Order>()
            {
                new Implementations.DomainObjects.Order() { Number = 39342, Item = "Macbeth" }
            };

            var result = repo.Edit(customerToEdit);

            result.Should().Be(2);

            var customer = repo.GetById(customerToEdit.Id);

            customer.Name.Should().Be("Sarah Moore");
        }

        [TestMethod]
        public void FindBy()
        {
            var repo = this.getRepo();

            var result = repo.FindBy(x => x.FirstName.Equals("Barry"), i => i.Orders);

            result.Count.Should().Be(1);
            result.Single().Name.Should().Be("Barry Jenkins");
        }

        [TestMethod]
        public void FirstOrDefault()
        {
            var repo = this.getRepo();

            var result = repo.FirstOrDefault(x => x.FirstName.Equals("Barry"), i => i.Orders);

            result.Name.Should().Be("Barry Jenkins");

            result = repo.FirstOrDefault(x => x.FirstName.Equals("Jackie"));

            result.Should().BeNull();
        }

        [TestMethod]
        public void GetById()
        {
            var repo = this.getRepo();

            var customerToGet = repo.FirstOrDefault(x => x.FirstName.Equals("Sarah"), i => i.Orders);

            customerToGet.Should().NotBeNull();

            var result = repo.GetById(customerToGet.Id);

            result.Name.Should().Be("Sarah Barnes");
        }

        [TestMethod]
        public void ProjectTo()
        {
            var repo = this.getRepo();

            var result = repo.GetAllAsLite();

            result.Count.Should().Be(2);
            result.First().GetType().Should().Be(typeof(Implementations.DomainObjects.LiteCustomer));
        }

        [TestMethod]
        public void ProjectToFiltered()
        {
            var repo = this.getRepo();

            var result = repo.GetAllAsLiteWithMultipleOrders();

            result.Count.Should().Be(1);
            result.First().GetType().Should().Be(typeof(Implementations.DomainObjects.LiteCustomer));
        }

        [TestMethod]
        public void SingleOrDefault()
        {
            var repo = this.getRepo();

            var result = repo.SingleOrDefault(x => x.FirstName.Equals("Barry"), i => i.Orders);

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

        private CustomerRepository getRepo() => getRepo(this.mapper);

        private CustomerRepository getRepo(IMapper mapper) => new CustomerRepository(new Implementations.Context.DbContext(), mapper);
    }
}
