using System;

namespace MappingRepository.Interfaces
{
    public interface IMappingRepositoryEntity<TKey> where TKey : IEquatable<TKey>
    {
        TKey Id { get; set; }
    }
}
