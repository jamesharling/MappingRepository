using MappingRepository.Interfaces;
using System;

namespace MappingRepository.Tests.Mocks.Entities
{
    internal class Order : IMappingRepositoryEntity<Guid>
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public int Number { get; set; }

        public string Item { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
