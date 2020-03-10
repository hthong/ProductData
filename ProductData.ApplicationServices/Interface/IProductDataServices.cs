using System.Collections.Generic;
using ProductData.ApplicationServices.Entity;

namespace ProductData.ApplicationServices.Interface
{
    public interface IProductDataServices
    {
        IList<MaxPriceItemByName> GetMaxPriceItems();
        MaxPriceItemByName GetMaxPriceItemByName(string name);
        IList<Item> GetAllItems();
        Item GetItemById(int id);
        Item UpdateItem(Item item);
        Item CreateItem(Item item);
        bool DeleteItem(int id);

    }
}
