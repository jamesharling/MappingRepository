using Moq;
using Moq.Language;
using Moq.Language.Flow;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace MappingRepository.Tests.Mocks
{
    public static class EFMockHelpers
    {
        public static IReturnsResult<TContext> ReturnsDbSet<TEntity, TContext>(this IReturns<TContext, DbSet<TEntity>> setup, TEntity[] entities)
            where TEntity : class
            where TContext : DbContext
        {
            var mockSet = CreateMockSet(entities);
            return setup.Returns(mockSet.Object);
        }

        public static IReturnsResult<TContext> ReturnsDbSet<TEntity, TContext>(this IReturns<TContext, DbSet<TEntity>> setup, IQueryable<TEntity> entities)
            where TEntity : class
            where TContext : DbContext
        {
            var mockSet = CreateMockSet(entities.ToList());
            return setup.Returns(mockSet.Object);
        }

        public static IReturnsResult<TContext> ReturnsDbSet<TEntity, TContext>(this IReturns<TContext, DbSet<TEntity>> setup, IEnumerable<TEntity> entities)
            where TEntity : class
            where TContext : DbContext
        {
            var mockSet = CreateMockSet(entities.ToList());
            return setup.Returns(mockSet.Object);
        }

        public static IReturnsResult<TContext> ReturnsDbSet<TEntity, TContext>(this IReturns<TContext, DbSet<TEntity>> setup, IList<TEntity> entities)
            where TEntity : class
            where TContext : DbContext
        {
            var mockSet = CreateMockSet(entities);
            return setup.Returns(mockSet.Object);
        }

        private static Mock<DbSet<T>> CreateMockSet<T>(IList<T> data) where T : class
        {
            var queryableData = data.AsQueryable();

            var mockSet = new Mock<DbSet<T>>();

            mockSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(queryableData.Provider);
            mockSet.As<IQueryable<T>>()
                .Setup(m => m.Expression)
                .Returns(queryableData.Expression);
            mockSet.As<IQueryable<T>>()
                .Setup(m => m.ElementType)
                .Returns(queryableData.ElementType);
            mockSet.As<IQueryable<T>>()
                .Setup(m => m.GetEnumerator())
                .Returns(queryableData.GetEnumerator());

            mockSet
                .Setup(m => m.Add(It.IsAny<T>()))
                .Callback<T>(data.Add);

            return mockSet;
        }
    }
}
