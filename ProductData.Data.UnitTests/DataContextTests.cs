using System.Linq;
using NUnit.Framework;
using ProductData.Data.SqlDataContext;

namespace ProductData.Data.UnitTests
{
    [TestFixture]
    public class DataContextTests
    {
        private ProductDataContext _dataContext;

        [OneTimeSetUp]
        public void Setup()
        {
            _dataContext = new ProductDataContext();
        }

        [Test]
        [Category("Integration")]
        public void Test_data_context_initialize_succeeded_and_seed_items_created()
        {
            Assert.That(_dataContext.Items.Any());
        }


        [OneTimeTearDown]
        public void TearDown()
        {
            _dataContext?.Dispose();
        }
    }
}
