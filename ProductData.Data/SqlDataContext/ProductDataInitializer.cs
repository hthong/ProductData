using System.Collections.Generic;
using System.Data.Entity;
using ProductData.Data.Models;

namespace ProductData.Data.SqlDataContext
{
    public class ProductDataInitializer: CreateDatabaseIfNotExists<ProductDataContext>
    {
        protected override void Seed(ProductDataContext context)
        {
            //Init Items
            var items = new List<Item>
            {
                new Item {Name = "Item 1", Cost = 100},
                new Item {Name = "Item 2", Cost = 200},
                new Item {Name = "Item 1", Cost = 250},
                new Item {Name = "Item 3", Cost = 300},
                new Item {Name = "Item 4", Cost = 50},
                new Item {Name = "Item 4", Cost = 40},
                new Item {Name = "Item 2", Cost = 200}
            };

            context.Items.AddRange(items);

            base.Seed(context);
        }

        
    }
}
