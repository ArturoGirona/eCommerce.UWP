using Library.eCommerce.Models;

namespace eCommerce.API.Database
{
    public class FakeDatabase
    {
        public static List<Product> Products { get; set; }

        public static List<Product> ProductsByQuantity = new List<Product>
        {
            new ProductByQuantity {Id = 1, Name = "Quantity1", Description = "Description 1" },
            new ProductByQuantity {Id = 2, Name = "Quantity2", Description = "Description 2" },
            new ProductByQuantity {Id = 3, Name = "Quantity3", Description = "Description 3" },
            new ProductByQuantity {Id = 4, Name = "Quantity4", Description = "Description 4" },
            new ProductByQuantity {Id = 5, Name = "Quantity5", Description = "Description 5" },
        };

        //    new List<ProductByQuantity> {

        //    new ProductByQuantity { Id = 1, Name = "Quantity1", Description="Description 1", Quantity=100, Price=1.23M},
        //    new ProductByQuantity { Id = 2, Name = "Quantity2", Description="Description 2", Quantity=100, Price=1.23M},
        //    new ProductByQuantity { Id = 3, Name = "Quantity3", Description="Description 3", Quantity=100, Price=1.23M},
        //    new ProductByQuantity { Id = 4, Name = "Quantity4", Description="Description 4", Quantity=100, Price=1.23M},
        //    new ProductByQuantity { Id = 5, Name = "Quantity5", Description="Description 54353", Quantity=100, Price=1.23M},
        //};            

        public static List<ProductByWeight> ProductsByWeight = new List<ProductByWeight>();
    }
}
