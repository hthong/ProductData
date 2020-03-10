using System.Collections.Generic;
using ProductData.ApplicationServices.Entity;

namespace ProductData.ApplicationServices.Interface
{
    public interface IProductDataApiServices
    {
        IList<MaxPriceItemByName> GetHighestCostItems();
        MaxPriceItemByName GetHighestCostItemByName(string name);

    }
}
