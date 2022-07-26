using Library.eCommerce.Models;

namespace eCommerce.API.Database
{
    public class FakeDatabase
    {
        public static List<Product> Inventory
        {
            get
            {
                var returnList = new List<Product>();
                ProductsByQuantity.ForEach(returnList.Add);
                ProductsByWeight.ForEach(returnList.Add);

                return returnList;
            }
        }

        public static List<ProductByQuantity> ProductsByQuantity = new List<ProductByQuantity>
        {
            new ProductByQuantity { Id = 1, Name = "Quantity1", Description = "Description 1", FoundIn = ProductType.INVENTORY , Price = 1.00M, Quantity = 1, BoGo = false},
            new ProductByQuantity { Id = 2, Name = "Quantity2", Description = "Description 2", FoundIn = ProductType.INVENTORY , Price = 2.00M, Quantity = 2, BoGo = true},
            new ProductByQuantity { Id = 3, Name = "Quantity3", Description = "Description 3", FoundIn = ProductType.INVENTORY , Price = 3.00M, Quantity = 3, BoGo = false},
            new ProductByQuantity { Id = 4, Name = "Quantity4", Description = "Description 4", FoundIn = ProductType.INVENTORY , Price = 4.00M, Quantity = 4, BoGo = true},
            new ProductByQuantity { Id = 5, Name = "Quantity5", Description = "Description 5", FoundIn = ProductType.INVENTORY , Price = 5.00M, Quantity = 5, BoGo = false},
        };

        public static List<ProductByWeight> ProductsByWeight = new List<ProductByWeight>
        {
            new ProductByWeight { Id = 6, Name = "Product3", Description="Description 1", FoundIn= ProductType.INVENTORY, Weight=1.23M, Price = 6.00M, BoGo=true},
            new ProductByWeight { Id = 7, Name = "Product2", Description="Description 2", FoundIn= ProductType.INVENTORY, Weight=1.23M, Price = 7.00M, BoGo=false},
            new ProductByWeight { Id = 8, Name = "Product3", Description="Description 3", FoundIn= ProductType.INVENTORY, Weight=1.23M, Price = 8.00M, BoGo=true},
            new ProductByWeight { Id = 9, Name = "Product4", Description="Description 4", FoundIn= ProductType.INVENTORY, Weight=1.23M, Price = 9.00M, BoGo=false},

        };
    }
}
