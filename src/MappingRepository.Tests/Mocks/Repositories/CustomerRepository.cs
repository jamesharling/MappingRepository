using AutoMapper;
using MappingRepository.Interfaces;
using System;

namespace MappingRepository.Tests.Mocks.Repositories
{
    public class CustomerRepository : MappingRepository<Entities.Customer, Guid, DomainObjects.Customer, Guid>
    {
        public CustomerRepository(IMappingRepositoryDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        { }
    }
}
