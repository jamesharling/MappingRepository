﻿using AutoMapper;
using MappingRepository.Tests.Implementations.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MappingRepository.Tests.Implementations.Repositories
{
    public class CustomerRepository : MappingRepository<Entities.Customer, DomainObjects.Customer, Guid>
    {
        public CustomerRepository(DbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        { }

        public IList<DomainObjects.Customer> Queryable()
        {
            return this.AsQueryable().ToList();
        }

        public IList<DomainObjects.Customer> FilteredQueryable()
        {
            return this.AsQueryable(x => x.FirstName.Equals("Barry")).ToList();
        }

        public IList<DomainObjects.LiteCustomer> GetAllAsLite()
        {
            return this.ProjectTo<DomainObjects.LiteCustomer>().ToList();
        }

        public IList<DomainObjects.Customer> GetAll()
        {
            return this.AsQueryable().ToList();
        }
    }
}
