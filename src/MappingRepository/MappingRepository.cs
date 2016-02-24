using AutoMapper;
using MappingRepository.Interfaces;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace MappingRepository
{
    public abstract class MappingRepository<TEntity, TEntityKey, TDestination, TDestinationKey>
        where TEntity : class, IMappingRepositoryEntity<TEntityKey>
        where TDestination : IMappingRepositoryDestination<TDestinationKey>
        where TEntityKey : IEquatable<TEntityKey>
        where TDestinationKey : IEquatable<TDestinationKey>
    {
        public MappingRepository(IMappingRepositoryDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.mapperConfig = this.mapper.ConfigurationProvider;
        }

        public virtual TDestinationKey Add(TDestination obj)
        {
            var entity = this.mapper.Map<TEntity>(obj);

            this.dbSet.Add(entity);

            this.dbContext.SaveChanges();

            return this.mapper.Map<TDestination>(entity).Id;
        }

        public virtual int AddRange(IEnumerable<TDestination> objs)
        {
            var entities = this.mapper.Map<IEnumerable<TEntity>>(objs);

            this.dbSet.AddRange(entities);

            return this.dbContext.SaveChanges();
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dbSet.Any(predicate);
        }

        public virtual void Delete(TEntityKey id)
        {
            this.dbSet.RemoveRange(this.dbSet.Where(x => x.Id.Equals(id)));
        }

        public virtual void Delete(TDestinationKey id)
        {
            this.dbSet.RemoveRange(this.dbSet.Where(x => x.Id.Equals(id)));
        }

        public virtual void DeleteRange(Expression<Func<TEntity, bool>> predicate)
        {
            this.dbSet.RemoveRange(this.dbSet.Where(predicate));
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

        public TDestination GetById(TEntityKey id)
        {
            return this.dbSet.Where(x => x.Id.Equals(id)).ProjectToSingleOrDefault<TDestination>(this.mapperConfig);
        }

        public TDestination GetById(TDestinationKey id)
        {
            return this.dbSet.Where(x => x.Id.Equals(id)).ProjectToSingleOrDefault<TDestination>(this.mapperConfig);
        }

        public IPagedList<TDestination> PagedList<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> order, bool isAscending, int pageNumber, int pageSize)
        {
            return this.dbSet.Where(predicate).OrderBy(order, isAscending).ProjectToQueryable<TDestination>(this.mapperConfig).ToPagedList(pageNumber, pageSize);
        }

        public TDestination SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dbSet.Where(predicate).ProjectToSingleOrDefault<TDestination>(this.mapperConfig);
        }

        protected IList<TDestination> GetAll()
        {
            return this.dbSet.ProjectToList<TDestination>(this.mapperConfig);
        }

        protected IQueryable<TCustomType> MapToCustomType<TCustomType>()
        {
            return this.dbSet.ProjectToQueryable<TCustomType>(this.mapperConfig);
        }

        private IMappingRepositoryDbContext dbContext;
        private IMapper mapper;
        private IConfigurationProvider mapperConfig;
        private DbSet<TEntity> dbSet => this.dbContext.Set<TEntity>();
    }
}
