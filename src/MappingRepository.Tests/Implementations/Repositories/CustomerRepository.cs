using AutoMapper;
using MappingRepository.Interfaces;
using MappingRepository.Tests.Implementations.Context;
using System;

namespace MappingRepository.Tests.Implementations.Repositories
{
    public class CustomerRepository : MappingRepository<Entities.Customer, DomainObjects.Customer, Guid>
    {
        public CustomerRepository(DbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        { }
    }
}
