using MappingRepository.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace MappingRepository.Tests.Implementations.Entities
{
    public class Order : IMappingRepositoryEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public int Number { get; set; }

        public string Item { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
