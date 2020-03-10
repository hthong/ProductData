using System.Web.Mvc;
using ProductData.ApplicationServices.Interface;

namespace ProductData.Web.Controllers
{
    public class MaxPriceItemController : Controller
    {
        private readonly IProductDataApiServices _services;

        public MaxPriceItemController(IProductDataApiServices services)
        {
            _services = services;
        }

        // GET: Product
        public ActionResult Index()
        {
            var model = _services.GetHighestCostItems();
            return View(model);
        }

        public ActionResult Details(string name)
        {

            var model = _services.GetHighestCostItemByName(name);
            return model == null
                ? (ActionResult) RedirectToAction("NotFound")
                : View(model);
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}