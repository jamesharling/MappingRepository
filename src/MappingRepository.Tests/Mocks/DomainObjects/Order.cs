using MappingRepository.Interfaces;
using System;

namespace MappingRepository.Tests.Mocks.DomainObjects
{
    internal class Order : IMappingRepositoryDestination<Guid>
    {
        public Guid Id { get; set; }

        public int Number { get; set; }

        public string Item { get; set; }
    }
}
