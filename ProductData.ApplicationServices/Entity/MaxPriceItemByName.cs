using System.ComponentModel.DataAnnotations;

namespace ProductData.ApplicationServices.Entity
{
    public class MaxPriceItemByName
    {
        public string Name { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
