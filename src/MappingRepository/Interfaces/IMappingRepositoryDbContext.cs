using System.Data.Entity;

namespace MappingRepository.Interfaces
{
    public interface IMappingRepositoryDbContext
    {
        int SaveChanges();

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}
