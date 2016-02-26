using Moq;
using System.Collections.Generic;
using System.Data.Entity;

namespace MappingRepository.Tests.Mocks
{
    public class MockedDbContext<T> : Mock<T> where T : DbContext
    {
        public Dictionary<string, object> Tables
        {
            get
            {
                return tables ?? (tables = new Dictionary<string, object>());
            }
        }

        private Dictionary<string, object> tables;
    }
}
