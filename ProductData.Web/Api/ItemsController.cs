using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
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
        public IHttpActionResult GetMaxPriceItemByNames()
        {
            try
            {
                var result = _productDataServices
                    .GetMaxPricesByItemName();
                return Ok(result);
            }
            catch (Exception e)
            {
                return InternalServerError(new Exception(e.Message));
            }
        }

        [HttpGet]
        [Route("group/name/agg/max(price)")]
        public IHttpActionResult GetMaxPriceItemByName(string name)
        {
            try
            {
                var result = _productDataServices
                    .GetMaxPricesByItemName()
                    .FirstOrDefault(q => q.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                return result != null ? (IHttpActionResult) Ok(result) : new NotFoundResult(this); 
            }
            catch (Exception e)
            {
                return InternalServerError(new Exception(e.Message));
            }
           
        }

    }
}
