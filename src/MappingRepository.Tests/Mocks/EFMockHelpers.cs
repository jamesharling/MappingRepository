using MappingRepository.Interfaces;
using MappingRepository.Tests.Implementations.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace MappingRepository.Tests.Mocks
{
    public static class EFMockHelpers
    {
        public static InMemoryDbSet<TEntity> FakeDbSet<TEntity>(IEnumerable<TEntity> data, bool clearDownExistingData = false) where TEntity : class
        {
            var fakeDbSet = new InMemoryDbSet<TEntity>();

            foreach (var item in data)
            {
                fakeDbSet.Add(item);
            }

            return fakeDbSet;
        }

        public static Mock<Implementations.Context.DbContext> GetMockedContext()
        {
            var mockedContext = new Mock<Implementations.Context.DbContext>();

            mockedContext.Setup(c => c.Set<Customer>()).Returns(FakeDbSet(Data.Customers));

            return mockedContext;
        }

        public static int GenerateIDs<TEntity>(Expression<Func<TEntity, Guid>> primaryKey, IDbSet<TEntity> entities) where TEntity : class, IMappingRepositoryEntity<Guid>
        {
            int newIds = 0;

            foreach (var item in entities.Where(x => x.Id == null || x.Id == Guid.Empty))
            {
                item.Id = Guid.NewGuid();

                newIds++;
            }

            return newIds;
        }
    }
}
