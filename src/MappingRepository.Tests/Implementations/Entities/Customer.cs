﻿using DelegateDecompiler;
using MappingRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MappingRepository.Tests.Implementations.Entities
{
    public class Customer : IMappingRepositoryEntity<Guid>
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        [Computed]
        public string Name => this.FirstName + " " + this.Surname;

        public ICollection<Order> Orders { get; set; }
    }
}
