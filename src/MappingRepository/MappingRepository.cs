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
        where TEntity : class, IMappingRepositoryEntity<TKey>
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

            var newEntity = this.dbSet.Add(entity);

            this.dbContext.SaveChanges();

            return entity.Id;
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
            this.dbSet.RemoveRange(this.dbSet.Where(x => x.Id.Equals(id)));

            return this.dbContext.SaveChanges();
        }

        public virtual int DeleteRange(Expression<Func<TEntity, bool>> predicate)
        {
            this.dbSet.RemoveRange(this.dbSet.Where(predicate));

            return this.dbContext.SaveChanges();
        }

        public virtual void Edit(TDestination obj)
        {
            var entity = this.GetById(obj.Id);

            this.mapper.Map(obj, entity);

            this.dbContext.SaveChanges();
        }

        public IList<TDestination> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dbSet.Where(predicate).ProjectToList<TDestination>(this.mapperConfig);
        }

        public TDestination FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dbSet.Where(predicate).ProjectToFirstOrDefault<TDestination>(this.mapperConfig);
        }

        public TDestination GetById(TKey id)
        {
            return this.dbSet.Where(x => x.Id.Equals(id)).ProjectToSingleOrDefault<TDestination>(this.mapperConfig);
        }

        public TDestination SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dbSet.Where(predicate).ProjectToSingleOrDefault<TDestination>(this.mapperConfig);
        }

        protected IQueryable<TDestination> AsQueryable()
        {
            return this.dbSet.ProjectToQueryable<TDestination>(this.mapperConfig);
        }

        protected IQueryable<TDestination> AsQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dbSet.Where(predicate).ProjectToQueryable<TDestination>(this.mapperConfig);
        }

        protected IQueryable<TCustomType> ProjectTo<TCustomType>()
        {
            return this.dbSet.ProjectToQueryable<TCustomType>(this.mapperConfig);
        }

        private DbContext dbContext;
        private IMapper mapper;

        private DbSet<TEntity> dbSet => this.dbContext.Set<TEntity>();

        private IConfigurationProvider mapperConfig => this.mapper.ConfigurationProvider;
    }
}
