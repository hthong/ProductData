using System.Collections.Generic;
using System.Web.Http;
using ProductData.ApplicationServices.Entity;
using ProductData.ApplicationServices.Interface;

namespace ProductData.Web.Api
{
    [RoutePrefix("api/items")]
    public class ItemsController : ApiController
    {
        private readonly IProductDataServices _productDataServices;

        public ItemsController(IProductDataServices productDataServices)
        
        {
            _productDataServices = productDataServices;
        }
     
       
        [HttpGet]
        [Route("group/name/agg/max(price)")]
        public IEnumerable<MaxPriceItemByName> GetMaxPriceItemByNames()
        {
            return _productDataServices.GetMaxPricesByItemName();
        }



    }
}
