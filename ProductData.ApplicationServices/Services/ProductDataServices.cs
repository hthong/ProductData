using System.Collections.Generic;
using System.Linq;
using ProductData.ApplicationServices.Entity;
using ProductData.ApplicationServices.Interface;
using ProductData.Data.SqlDataContext;
using PersistenceItem = ProductData.Data.Models.Item;

namespace ProductData.ApplicationServices.Services
{
    public class ProductDataServices : IProductDataServices
    {
        private readonly ProductDataContext _dataContext;

        public ProductDataServices(ProductDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IList<MaxPriceItemByName> GetMaxPriceItems()
        {
            var result = _dataContext
                .Items
                .GroupBy(
                    g => g.Name,
                    (key, item) => new MaxPriceItemByName
                    {
                        Name = key,
                        MaxPrice = item.Max(q => q.Cost)
                    }
                );

            return result.ToList();

        }

        public MaxPriceItemByName GetMaxPriceItemByName(string name)
        {
            var result = _dataContext
                .Items
                .Where(q => q.Name == name)
                .GroupBy(
                    g => g.Name,
                    (key, item) => new MaxPriceItemByName
                    {
                        Name = key,
                        MaxPrice = item.Max(q => q.Cost)
                    }
                ).FirstOrDefault();

            return result;
        }

        public IList<Item> GetAllItems()
        {
            return _dataContext.Items.AsNoTracking().Select(Map).ToList();
        }

        public Item GetItemById(int id)
        {
            var result = GetPersistenceItemById(id);
            return Map(result);
        }

        public Item UpdateItem(Item item)
        {
            var updateItem = GetPersistenceItemById(item.Id);
            if (updateItem != null)
            {
                updateItem.Name = item.Name;
                updateItem.Cost = item.Cost;
            }

            _dataContext.SaveChanges();

            return Map(updateItem);

        }

        public Item CreateItem(Item item)
        {
            if (item == null) return null;
            var insertItem = new PersistenceItem {Name = item.Name, Cost = item.Cost};

            var result = _dataContext.Items.Add(insertItem);
            _dataContext.SaveChanges();

            return Map(result);
        }


        //Delete or Inactivate?
        public bool DeleteItem(int id)
        {
            var deleteItem = GetPersistenceItemById(id);
            if (deleteItem == null) return false;

            _dataContext.Items.Remove(deleteItem);
            _dataContext.SaveChanges();

            return true;
        }

        private PersistenceItem GetPersistenceItemById(int id)
        {
            return _dataContext.Items.FirstOrDefault(q => q.Id == id);
        }


        //TODO: Mapper library like AutoMapper 
        #region Mapper

        private static Item Map(PersistenceItem item)
        {
            if (item == null) return null;

            return new Item
            {
                Id = item.Id,
                Name = item.Name,
                Cost = item.Cost
            };
        }

        #endregion

    }
}
