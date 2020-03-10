using System;
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
        [Route("")]
        public IHttpActionResult GetAllItems()
        {
            var action = new Func<IHttpActionResult>(() =>
            {
                var result = _productDataServices.GetAllItems();
                return Ok(result);
            });

            return ErrorHandlerWrapper(action);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetItemById([FromUri]int id)
        {
            var action = new Func<IHttpActionResult>(() =>
            {
                var result = _productDataServices.GetItemById(id);
                return result != null ? (IHttpActionResult)Ok(result) : new NotFoundResult(this);
            });

            return ErrorHandlerWrapper(action);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateItem([FromBody]Item createItem)
        {
            var action = new Func<IHttpActionResult>(() =>
            {
                var result = _productDataServices.CreateItem(createItem);
                var uri = new Uri(Request.RequestUri, result.Id.ToString());
                return Created(uri, result);
            });

            return ErrorHandlerWrapper(action);
        }

        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdateItem([FromUri]int id, [FromBody] Item updateItem)
        {
            var action = new Func<IHttpActionResult>(() =>
            {
                updateItem.Id = id;
                var result = _productDataServices.UpdateItem(updateItem);
                return result != null ? (IHttpActionResult)Ok(result) : new NotFoundResult(this);
            });

            return ErrorHandlerWrapper(action);
        }

        
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteItem([FromUri]int id)
        {
            var action = new Func<IHttpActionResult>(() =>
            {
                var result = _productDataServices.DeleteItem(id);
                return result ?  (IHttpActionResult)Ok("") : new NotFoundResult(this);
            });

            return ErrorHandlerWrapper(action);
        }


        [HttpGet]
        [Route("group/name/agg/max(price)")]
        public IHttpActionResult GetMaxPriceItemByNames()
        {
            var action = new Func<IHttpActionResult>(() =>
                {
                    var result = _productDataServices.GetMaxPriceItems();
                    return Ok(result);
                });

            return ErrorHandlerWrapper(action);
        }

        [HttpGet]
        [Route("group/name/agg/max(price)")]
        public IHttpActionResult GetMaxPriceItemByName(string name)
        {

            var action = new Func<IHttpActionResult>(() =>
            {
                var result = _productDataServices
                    .GetMaxPriceItems()
                    .FirstOrDefault(q => q.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                return result != null ? (IHttpActionResult)Ok(result) : new NotFoundResult(this);
            });

            return ErrorHandlerWrapper(action);
        }

        private IHttpActionResult ErrorHandlerWrapper(Func<IHttpActionResult> func)
        {
            try
            {
                return func();
            }
            catch (Exception e)
            {
                return InternalServerError(new Exception(e.Message));
            }
        }

    }
}
