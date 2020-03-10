using System.Collections.Generic;
using ProductData.ApplicationServices.Entity;

namespace ProductData.ApplicationServices.Interface
{
    public interface IProductDataServices
    {
        IList<MaxPriceItemByName> GetMaxPricesByItemName();
    }
}
