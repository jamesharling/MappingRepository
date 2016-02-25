using MappingRepository.Interfaces;
using MappingRepository.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MappingRepository.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void CanAddEntity()
        {
            var repo = new CustomerRepository(new DbContext(), Mapper.GetConfig())
        }
    }
}
