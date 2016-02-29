using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace MappingRepository
{
    /// <summary>
    /// Have your database context implement this interface.
    /// </summary>
    public interface IMappingRepositoryContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
