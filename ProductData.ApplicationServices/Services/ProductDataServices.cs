using System.Collections.Generic;
using System.Linq;
using ProductData.ApplicationServices.Entity;
using ProductData.ApplicationServices.Interface;
using ProductData.Data.SqlDataContext;

namespace ProductData.ApplicationServices.Services
{
    public class ProductDataServices : IProductDataServices
    {
        private readonly ProductDataContext _dataContext;

        public ProductDataServices(ProductDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IList<MaxPriceItemByName> GetMaxPricesByItemName()
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
    }
}
