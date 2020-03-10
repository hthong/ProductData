using System.Data.Entity;
using ProductData.Data.Models;

namespace ProductData.Data.SqlDataContext
{
    public class ProductDataContext: DbContext
    {
        public ProductDataContext() : this("ProductData")
        {
            
        }

        public ProductDataContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Database.SetInitializer(new ProductDataInitializer());
        }

        public virtual DbSet<Item> Items { get; set; }
       
    }
}
