using System;

namespace MappingRepository.Interfaces
{
    public interface IMappingRepositoryDestination<TKey> where TKey : IEquatable<TKey>
    {
        TKey Id { get; set; }
    }
}
