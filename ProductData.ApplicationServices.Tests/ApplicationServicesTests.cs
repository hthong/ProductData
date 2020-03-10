using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moq;
using NUnit.Framework;
using ProductData.ApplicationServices.Entity;
using ProductData.ApplicationServices.Interface;
using ProductData.ApplicationServices.Services;
using ProductData.Data.SqlDataContext;
using PersistenceItem = ProductData.Data.Models.Item;

namespace ProductData.ApplicationServices.Tests
{
    [TestFixture]
    public class ApplicationServicesTests
    {
        private IProductDataServices _applicationServices;

        [OneTimeSetUp]
        public void Setup()
        {
            var mockDbItems = new Mock<DbSet<PersistenceItem>>();
            mockDbItems.As<IQueryable<PersistenceItem>>().Setup(m => m.Provider).Returns(MockItems.Provider);
            mockDbItems.As<IQueryable<PersistenceItem>>().Setup(m => m.Expression).Returns(MockItems.Expression);
            mockDbItems.As<IQueryable<PersistenceItem>>().Setup(m => m.ElementType).Returns(MockItems.ElementType);
            mockDbItems.As<IQueryable<PersistenceItem>>().Setup(s => s.GetEnumerator()).Returns(MockItems.GetEnumerator);
            mockDbItems.Setup(m => m.AsNoTracking()).Returns(mockDbItems.Object);
            mockDbItems.Setup(d => d.Add(It.IsAny<PersistenceItem>()))
                .Returns((PersistenceItem u) => u)
                .Callback<PersistenceItem>(s => ItemList.Add(s));
            mockDbItems.Setup(d => d.Remove(It.IsAny<PersistenceItem>()))
                .Returns((PersistenceItem u) => u)
                .Callback<PersistenceItem>(s => ItemList.Remove(s));
             
            var mockDataContext = new Mock<ProductDataContext>();
            mockDataContext.Setup(s => s.Items).Returns(mockDbItems.Object);

            _applicationServices = new ProductDataServices(mockDataContext.Object);
        }

        [Test]
        [Order(1)]
        public void Get_all_items_should_return_correct_count()
        {
            var expectedCount = MockItems.Count();
            var result = _applicationServices.GetAllItems().Count;

            Assert.That(expectedCount == result);
        }

        
        [Test]
        [TestCase(1)]
        [Order(2)]
        public void Get_item_by_id_should_return_correct_item(int id)
        {
            var expectedItem = MockItems.FirstOrDefault(q=>q.Id == id);
            var result = _applicationServices.GetItemById(id);

            Assert.IsNotNull(result);
            Assert.IsNotNull(expectedItem);
            Assert.That(expectedItem.Id == result.Id);
            Assert.That(expectedItem.Name == result.Name);
            Assert.That(expectedItem.Cost == result.Cost);
        }

        [Test]
        [TestCase("Item 999", 1000)]
        [Order(3)]
        public void Create_item_should_return_created_item_with_list_inc_one(string name, decimal price)
        {
            var currentListCount = ItemList.Count;

            var result = _applicationServices.CreateItem(new Item{Name = name, Cost = price});

            Assert.IsNotNull(result);
            Assert.That(result.Name == name);
            Assert.That(result.Cost == price);
            Assert.That(currentListCount +1 == ItemList.Count);
        }

        [Test]
        [TestCase(1, "Item 1", 1001)]
        [Order(998)]
        public void Update_item_should_return_correct_count(int id, string name, decimal price)
        {
            var result = _applicationServices.UpdateItem(new Item{Id = id, Name = name, Cost = price});

            Assert.IsNotNull(result);
            Assert.That(result.Name == name);
            Assert.That(result.Cost == price);
        }

        [Test]
        [TestCase(1)]
        [Order(999)]
        public void Delete_item_should_return_true_with_list_desc_one(int id)
        {
            var currentListCount = ItemList.Count;
            var result = _applicationServices.DeleteItem(id);

            Assert.IsTrue(result);
            Assert.That(currentListCount - 1 == ItemList.Count);
        }

        [Test]
        [Order(5)]
        public void Group_item_by_max_price_should_return_correct_value()
        {
            var result = _applicationServices.GetMaxPriceItems();

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

        #region Mock data
        private static readonly IList<PersistenceItem> ItemList = new List<PersistenceItem>
        {
            new PersistenceItem {Id = 1, Name = "Item 1", Cost = 100},
            new PersistenceItem {Id = 2, Name = "Item 2", Cost = 200},
            new PersistenceItem {Id = 3, Name = "Item 1", Cost = 250},
            new PersistenceItem {Id = 4, Name = "Item 3", Cost = 300},
            new PersistenceItem {Id = 5, Name = "Item 4", Cost = 50},
            new PersistenceItem {Id = 6, Name = "Item 4", Cost = 40},
            new PersistenceItem {Id = 7, Name = "Item 2", Cost = 200}
        };

        private static IQueryable<PersistenceItem> MockItems => ItemList.AsQueryable();

        #endregion
        
    }
}
