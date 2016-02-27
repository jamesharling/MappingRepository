using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace MappingRepository.Tests.Mocks
{
    /// <summary>
    /// The in-memory database set, taken from Microsoft's online example
    /// (http://msdn.microsoft.com/en-us/ff714955.aspx) and modified to be based on DbSet instead of ObjectSet.
    /// </summary>
    /// <typeparam name="TEntity">The type of DbSet.</typeparam>
    public class InMemoryDbSet<TEntity> : DbSet<TEntity>, IDbSet<TEntity> where TEntity : class
    {
        /// <summary>
        /// Creates an instance of the InMemoryDbSet using the default static backing store.This
        /// means that data persists between test runs, like it would do with a database unless you
        /// cleared it down.
        /// </summary>
        public InMemoryDbSet()
            : this(true)
        {
        }

        /// <summary>
        /// This constructor allows you to pass in your own data store, instead of using the static
        /// backing store.
        /// </summary>
        /// <param name="data">A place to store data.</param>
        public InMemoryDbSet(HashSet<TEntity> data)
        {
            _nonStaticData = data;
        }

        /// <summary>
        /// Creates an instance of the InMemoryDbSet using the default static backing store.This
        /// means that data persists between test runs, like it would do with a database unless you
        /// cleared it down.
        /// </summary>
        /// <param name="clearDownExistingData"></param>
        public InMemoryDbSet(bool clearDownExistingData)
        {
            if (clearDownExistingData)
            {
                Clear();
            }
        }

        public Type ElementType
        {
            get { return Data.AsQueryable().ElementType; }
        }

        public Expression Expression
        {
            get { return Data.AsQueryable().Expression; }
        }

        public Func<IEnumerable<TEntity>, object[], TEntity> FindFunction
        {
            get { return _findFunction; }
            set { _findFunction = value; }
        }

        public override ObservableCollection<TEntity> Local
        {
            get { return new ObservableCollection<TEntity>(Data); }
        }

        public IQueryProvider Provider
        {
            get { return Data.AsQueryable().Provider; }
        }

        public override TEntity Add(TEntity entity)
        {
            Data.Add(entity);

            return entity;
        }

        public override IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                this.Add(entity);
            }

            return entities;
        }

        public override TEntity Attach(TEntity entity)
        {
            Data.Add(entity);
            return entity;
        }

        public void Clear()
        {
            Data.Clear();
        }

        public override TDerivedEntity Create<TDerivedEntity>()
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public override TEntity Create()
        {
            return Activator.CreateInstance<TEntity>();
        }

        public override TEntity Find(params object[] keyValues)
        {
            if (FindFunction == null)
            {
                return FindFunction(Data, keyValues);
            }
            else
            {
                throw new NotImplementedException("Derive from InMemoryDbSet and override Find, or provide a FindFunction.");
            }
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator1();
        }

        public override TEntity Remove(TEntity entity)
        {
            Data.Remove(entity);

            return entity;
        }

        public override IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                this.Remove(entity);
            }

            return entities;
        }


        private static readonly HashSet<TEntity> _staticData = new HashSet<TEntity>();
        private readonly HashSet<TEntity> _nonStaticData;
        private Func<IEnumerable<TEntity>, object[], TEntity> _findFunction;

        /// <summary>
        /// The non static backing store data for the InMemoryDbSet.
        /// </summary>
        private HashSet<TEntity> Data
        {
            get
            {
                if (_nonStaticData == null)
                {
                    return _staticData;
                }
                else
                {
                    return _nonStaticData;
                }
            }
        }

        private IEnumerator GetEnumerator1()
        {
            return Data.GetEnumerator();
        }
    }
}
