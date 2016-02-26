using MappingRepository.Interfaces;
using System;
using System.Collections.Generic;

namespace MappingRepository.Tests.Implementations.DomainObjects
{
    public class Customer : IMappingRepositoryDestination<Guid>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        //public IEnumerable<Order> Orders { get; set; }
    }
}
