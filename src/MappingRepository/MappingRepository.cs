using AutoMapper;
using MappingRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace MappingRepository
{
    /// <summary>
    /// A MappingRepository instance represents a repository that automatically maps incoming and
    /// outgoing objects to and from your desired types. A classic example of use would be using
    /// repositories derived from <see cref="MappingRepository{TEntity, TDestination, TKey}"/> to
    /// create abstraction, allowing you to implement DDD.
    /// </summary>
    /// <typeparam name="TEntity">The type of the underlying database entity.</typeparam>
    /// <typeparam name="TDestination">The type of your abstracted domain layer object.</typeparam>
    /// <typeparam name="TKey">The type of the common key that your entities share.</typeparam>
    public abstract class MappingRepository<TEntity, TDestination, TKey>
        where TEntity : class, IMappingRepositoryEntity<TKey>, new()
        where TDestination : IMappingRepositoryDestination<TKey>
        where TKey : IEquatable<TKey>
    {
        private DbContext dbContext;

        private IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingRepository{TEntity, TDestination,
        /// TKey}"/> class. Call this constructor when deriving from <see
        /// cref="MappingRepository{TEntity, TDestination, TKey}"/>.
        /// </summary>
        /// <param name="dbContext">The Entity Framework context to use for database access.</param>
        /// <param name="mapper">
        /// An AutoMapper IMapper instance, containing the maps and configuration for mapping operations.
        /// </param>
        protected MappingRepository(DbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        private DbSet<TEntity> dbSet => this.dbContext.Set<TEntity>();

        private IConfigurationProvider mapperConfig => this.mapper.ConfigurationProvider;

        /// <summary>
        /// Adds the given entity into context.
        /// </summary>
        /// <param name="obj">The entity to add.</param>
        /// <returns>Returns the newly-generated ID of the inserted entity.</returns>
        public virtual TKey Add(TDestination obj)
        {
            var entity = this.mapper.Map<TEntity>(obj);

            var result = this.dbSet.Add(entity);

            this.dbContext.SaveChanges();

            return result.Id;
        }

        /// <summary>
        /// Adds the given collection of entities into context.
        /// </summary>
        /// <param name="objs">The collection of entities to add.</param>
        /// <returns>The number of successfully inserted entities.</returns>
        public virtual int AddRange(IEnumerable<TDestination> objs)
        {
            var entities = this.mapper.Map<IEnumerable<TEntity>>(objs);

            this.dbSet.AddRange(entities);

            return this.dbContext.SaveChanges();
        }

        /// <summary>
        /// Determines whether any element of a sequence satisfies a condition.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// true if any elements in the source sequence pass the test in the specified predicate;
        /// otherwise, false.
        /// </returns>
        public virtual bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dbSet.Any(predicate);
        }

        /// <summary>
        /// Removes the specified entity from the context.
        /// </summary>
        /// <param name="id">The unique ID of the entity to delete.</param>
        /// <returns>The number of deleted entities.</returns>
        public virtual int Delete(TKey id)
        {
            var entity = new TEntity()
            {
                Id = id
            };

            this.dbContext.Entry(entity).State = EntityState.Deleted;

            return this.dbContext.SaveChanges();
        }

        /// <summary>
        /// Removes the given collection of entities from the context.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The number of deleted entities.</returns>
        public virtual int DeleteRange(Expression<Func<TEntity, bool>> predicate)
        {
            this.dbSet.RemoveRange(this.dbSet.Where(predicate));

            return this.dbContext.SaveChanges();
        }

        public virtual int Edit(TDestination obj)
        {
            var entity = this.dbSet.Find(obj.Id);

            entity = this.mapper.Map(obj, entity);

            return this.dbContext.SaveChanges();
        }

        /// <summary>
        /// Returns all elements of a sequence that satisfy a specified condition or an empty
        /// collection if no elements satisfy the condition.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// An empty collection if no element passes the test specified by predicate; otherwise, all
        /// elements in source that pass the test specified by predicate.
        /// </returns>
        public IList<TDestination> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dbSet.AsNoTracking().Where(predicate).ProjectToList<TDestination>(this.mapperConfig);
        }

        /// <summary>
        /// Returns the first element of a sequence that satisfies a specified condition or a
        /// default value if no such element is found.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// default(TDestination) if source is empty or if no element passes the test specified by
        /// predicate; otherwise, the first element in source that passes the test specified by predicate.
        /// </returns>
        public TDestination FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dbSet.AsNoTracking().Where(predicate).ProjectToFirstOrDefault<TDestination>(this.mapperConfig);
        }

        public TDestination GetById(TKey id)
        {
            return this.dbSet.AsNoTracking().Where(x => x.Id.Equals(id)).ProjectToSingleOrDefault<TDestination>(this.mapperConfig);
        }

        /// <summary>
        /// Returns the only element of a sequence that satisfies a specified condition or a default
        /// value if no such element exists; this method throws an exception if more than one
        /// element satisfies the condition.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// The single element of the input sequence that satisfies the condition in predicate, or
        /// default(TDestination) if no such element is found.
        /// </returns>
        public TDestination SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dbSet.AsNoTracking().Where(predicate).ProjectToSingleOrDefault<TDestination>(this.mapperConfig);
        }

        /// <summary>
        /// Presents a <see cref="IQueryable{TDestination}"/> object for consumption in your derived repository. 
        /// </summary>
        /// <returns></returns>
        protected IQueryable<TDestination> AsQueryable()
        {
            return this.dbSet.AsNoTracking().ProjectToQueryable<TDestination>(this.mapperConfig);
        }

        protected IQueryable<TDestination> AsQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dbSet.AsNoTracking().Where(predicate).ProjectToQueryable<TDestination>(this.mapperConfig);
        }

        protected IQueryable<TCustomType> ProjectTo<TCustomType>()
        {
            return this.dbSet.AsNoTracking().ProjectToQueryable<TCustomType>(this.mapperConfig);
        }
    }
}
