using MappingRepository.Interfaces;
using System;
using System.Collections.Generic;

namespace MappingRepository.Tests.Mocks.Entities
{
    internal class Customer : IMappingRepositoryEntity<Guid>
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
