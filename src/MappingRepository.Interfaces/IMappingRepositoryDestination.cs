using System;

namespace MappingRepository.Interfaces
{
    /// <summary>
    /// Have your destination objects implement this interface.
    /// </summary>
    /// <typeparam name="TKey">The type of the key that your destination objects use.</typeparam>
    public interface IMappingRepositoryDestination<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// The key that uniquely identifies your destination objects.
        /// </summary>
        TKey Id { get; set; }
    }
}
