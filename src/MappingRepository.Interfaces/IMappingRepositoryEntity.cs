using System;

namespace MappingRepository.Interfaces
{
    /// <summary>
    /// Have your entities implement this interface.
    /// </summary>
    /// <typeparam name="TKey">The type of the key that your entities use.</typeparam>
    public interface IMappingRepositoryEntity<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// The key that uniquely identifies your entities within their sets.
        /// </summary>
        TKey Id { get; set; }
    }
}
