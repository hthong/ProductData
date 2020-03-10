using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moq;
using NUnit.Framework;
using ProductData.ApplicationServices.Entity;
using ProductData.ApplicationServices.Interface;
using ProductData.ApplicationServices.Services;
using ProductData.Data.Models;
using ProductData.Data.SqlDataContext;

namespace ProductData.ApplicationServices.Tests
{
    [TestFixture]
    public class ApplicationServicesTests
    {
        private IProductDataServices _applicationServices;

        [OneTimeSetUp]
        public void Setup()
        {
            var mockDbItems = new Mock<DbSet<Item>>();
            mockDbItems.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(MockItems.Provider);
            mockDbItems.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(MockItems.Expression);
            mockDbItems.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(MockItems.ElementType);
            mockDbItems.As<IQueryable<Item>>().Setup(s => s.GetEnumerator()).Returns(MockItems.GetEnumerator);

            var mockDataContext = new Mock<ProductDataContext>();
            mockDataContext.Setup(s => s.Items).Returns(mockDbItems.Object);

            _applicationServices = new ProductDataServices(mockDataContext.Object);
        }

        [Test]
        public void Group_item_by_max_price_should_return_correct_value()
        {

            var result = _applicationServices.GetMaxPricesByItemName();

            var item1 = result.FirstOrDefault(q => q.Name.Equals("Item 1", StringComparison.OrdinalIgnoreCase));
            Assert.That(VerifyMaxPriceItem(item1, 250));

            var item2 = result.FirstOrDefault(q => q.Name.Equals("Item 2", StringComparison.OrdinalIgnoreCase));
            Assert.That(VerifyMaxPriceItem(item2, 200));

            var item3 = result.FirstOrDefault(q => q.Name.Equals("Item 3", StringComparison.OrdinalIgnoreCase));
            Assert.That(VerifyMaxPriceItem(item3, 300));

            var item4 = result.FirstOrDefault(q => q.Name.Equals("Item 4", StringComparison.OrdinalIgnoreCase));
            Assert.That(VerifyMaxPriceItem(item4, 50));
        }


        private static bool VerifyMaxPriceItem(MaxPriceItemByName item, decimal maxPrice)
        {
            return item != null && decimal.Compare(item.MaxPrice, maxPrice) == 0;
        }


        private static readonly IQueryable<Item> MockItems =
             new List<Item>
            {
                new Item {Id = 1, Name = "Item 1", Cost = 100},
                new Item {Id = 2, Name = "Item 2", Cost = 200},
                new Item {Id = 3, Name = "Item 1", Cost = 250},
                new Item {Id = 4, Name = "Item 3", Cost = 300},
                new Item {Id = 5, Name = "Item 4", Cost = 50},
                new Item {Id = 6, Name = "Item 4", Cost = 40},
                new Item {Id = 7, Name = "Item 2", Cost = 200}
            }.AsQueryable();
        
    }
}
