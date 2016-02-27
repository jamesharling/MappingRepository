using MappingRepository.Tests.Implementations.Entities;
using Moq;
using Moq.Language;
using Moq.Language.Flow;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MappingRepository.Tests.Mocks
{
    public static class EFMockHelpers
    {
        public static Mock<DbSet<T>> CreateMockSet<T>(List<T> data) where T : class
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
                .Setup(set => set.Add(It.IsAny<T>()))
                .Callback<T>(data.Add);
            mockSet
                .Setup(set => set.AddRange(It.IsAny<IEnumerable<T>>()))
                .Callback<IEnumerable<T>>(data.AddRange);
            mockSet
                .Setup(set => set.Remove(It.IsAny<T>()))
                .Callback<T>(t => data.Remove(t));
            mockSet
                .Setup(set => set.RemoveRange(It.IsAny<IEnumerable<T>>()))
                .Callback<IEnumerable<T>>(ts => data.RemoveRange(0, 2));

            return mockSet;
        }

        public static Mock<Implementations.Context.DbContext> GetMockedContext()
        {
            var mockedContext = new Mock<Implementations.Context.DbContext>();

            mockedContext.Setup(c => c.Set<Customer>()).ReturnsDbSet(Data.Customers);

            return mockedContext;
        }

        public static IReturnsResult<TContext> ReturnsDbSet<TEntity, TContext>(this IReturns<TContext, DbSet<TEntity>> setup, List<TEntity> entities)
                            where TEntity : class
            where TContext : DbContext
        {
            var mockSet = CreateMockSet(entities);
            return setup.Returns(mockSet.Object);
        }
    }
}
