﻿using MappingRepository.Interfaces;
using System;

namespace MappingRepository.Tests.Implementations.DomainObjects
{
    public class Order : IMappingRepositoryDestination<Guid>
    {
        public Guid Id { get; set; }

        public string Item { get; set; }

        public int Number { get; set; }
    }
}
