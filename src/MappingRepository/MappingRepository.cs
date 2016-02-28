using AutoMapper;
using MappingRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace MappingRepository
{
    public abstract class MappingRepository<TEntity, TDestination, TKey>
        where TEntity : class, IMappingRepositoryEntity<TKey>, new()
        where TDestination : IMappingRepositoryDestination<TKey>
        where TKey : IEquatable<TKey>
    {
        public MappingRepository(DbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public virtual TKey Add(TDestination obj)
        {
            var entity = this.mapper.Map<TEntity>(obj);

            var result = this.dbSet.Add(entity);

            this.dbContext.SaveChanges();

            return result.Id;
        }

        public virtual int AddRange(IEnumerable<TDestination> objs)
        {
            var entities = this.mapper.Map<IEnumerable<TEntity>>(objs);

            this.dbSet.AddRange(entities);

            return this.dbContext.SaveChanges();
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dbSet.Any(predicate);
        }

        public virtual int Delete(TKey id)
        {
            var entity = new TEntity()
            {
                Id = id
            };

            this.dbContext.Entry(entity).State = EntityState.Deleted;

            return this.dbContext.SaveChanges();
        }

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

        public IList<TDestination> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dbSet.AsNoTracking().Where(predicate).ProjectToList<TDestination>(this.mapperConfig);
        }

        public TDestination FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dbSet.AsNoTracking().Where(predicate).ProjectToFirstOrDefault<TDestination>(this.mapperConfig);
        }

        public TDestination GetById(TKey id)
        {
            return this.dbSet.AsNoTracking().Where(x => x.Id.Equals(id)).ProjectToSingleOrDefault<TDestination>(this.mapperConfig);
        }

        public TDestination SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dbSet.AsNoTracking().Where(predicate).ProjectToSingleOrDefault<TDestination>(this.mapperConfig);
        }

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

        private DbContext dbContext;
        private IMapper mapper;

        private DbSet<TEntity> dbSet => this.dbContext.Set<TEntity>();

        private IConfigurationProvider mapperConfig => this.mapper.ConfigurationProvider;
    }
}
