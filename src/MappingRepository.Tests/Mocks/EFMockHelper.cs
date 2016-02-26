using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace MappingRepository.Tests.Mocks
{
    public static class EFMockHelper
    {
        public static MockedDbContext<T> GetMockContext<T>() where T : DbContext
        {
            var instance = new MockedDbContext<T>();
            instance.MockTables();
            return instance;
        }

        public static Mock<DbSet<T>> MockDbSet<T>(List<T> data) where T : class
        {
            var dbSet = new Mock<DbSet<T>>();

            dbSet.As<IQueryable<T>>().Setup(q => q.Provider).Returns(() => data.AsQueryable().Provider);
            dbSet.As<IQueryable<T>>().Setup(q => q.Expression).Returns(() => data.AsQueryable().Expression);
            dbSet.As<IQueryable<T>>().Setup(q => q.ElementType).Returns(() => data.AsQueryable().ElementType);
            dbSet.As<IQueryable<T>>().Setup(q => q.GetEnumerator()).Returns(() => data.AsQueryable().GetEnumerator());

            dbSet.Setup(s => s.Add(It.IsAny<T>())).Callback<T>(x => data.Add(x)).Returns<T>(t => t);
            dbSet.Setup(s => s.AddRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(t => data.AddRange(t)).Returns<IEnumerable<T>>(t => t);
            //dbSet.Setup(s => s.Any(It.IsAny<Func<T, bool>>())).Callback<Func<T, bool>>(x => data.Any(x)).Returns<bool>(t => t);
            dbSet.Setup(s => s.Remove(It.IsAny<T>())).Callback<T>(t => data.Remove(t));
            dbSet.Setup(s => s.RemoveRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(ts => { foreach (var t in ts) data.Remove(t); });

            return dbSet;
        }

        public static void MockTables<T>(this MockedDbContext<T> mockedContext) where T : DbContext
        {
            Type contextType = typeof(T);
            var dbSetProperties = contextType.GetProperties().Where(prop => (prop.PropertyType.IsGenericType) && prop.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));
            foreach (var prop in dbSetProperties)
            {
                var dbSetGenericType = prop.PropertyType.GetGenericArguments()[0];
                Type listType = typeof(List<>).MakeGenericType(dbSetGenericType);
                var listForFakeTable = Activator.CreateInstance(listType);
                var parameter = Expression.Parameter(contextType);
                var body = Expression.PropertyOrField(parameter, prop.Name);
                var lambdaExpression = Expression.Lambda<Func<T, object>>(body, parameter);
                var method = typeof(EFMockHelper).GetMethod("MockDbSet").MakeGenericMethod(dbSetGenericType);
                mockedContext.Setup(lambdaExpression).Returns(method.Invoke(null, new[] { listForFakeTable }));
                mockedContext.Tables.Add(prop.Name, listForFakeTable);
            }
        }
    }
}
