using MappingRepository.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MappingRepository.Tests.Implementations.Entities
{
    public class Order : IMappingRepositoryEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }

        public int Number { get; set; }

        public string Item { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
