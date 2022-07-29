using Library.eCommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Library.eCommerce.Services;
using eCommerce.API.Database;

namespace eCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        //private Filebase FakeDatabase = Filebase.Current;

        private readonly ILogger<ProductController> _logger;
        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<Product> Get()
        {
            return FakeDatabase.Inventory;
        }

        [HttpPost("Delete")]
        public int Delete(DeleteParams pList)
        //public int Delete(int id, ProductType searchIn = ProductType.INVENTORY, string cartName = "")
        {
            int id = pList.id;
            ProductType searchIn = pList.searchIn;
            string cartName = pList.cartName;

            var itemToDelete = FakeDatabase.Inventory.FirstOrDefault(i => i.Id == id && i.FoundIn == searchIn);
            if (itemToDelete == default || itemToDelete == null)
            {
                Console.WriteLine("The item has not been found and cannot be deleted");
                return id;
            }


            if (searchIn == ProductType.INVENTORY)
            {
                var deleteFromCart = FakeDatabase.Inventory.FirstOrDefault(i => i.Id == id && i.FoundIn == ProductType.CART);

                if (deleteFromCart is ProductByQuantity)
                    FakeDatabase.ProductsByQuantity.Remove((ProductByQuantity)(deleteFromCart ?? new ProductByQuantity()));
                else if (deleteFromCart is ProductByWeight)
                    FakeDatabase.ProductsByWeight.Remove((ProductByWeight)(deleteFromCart ?? new ProductByWeight()));
            }
            else
            {
                var inventoryItem = FakeDatabase.Inventory.FirstOrDefault(i => i.Id == id && i.FoundIn == ProductType.INVENTORY);
                if (itemToDelete.GetType().Equals(typeof(ProductByWeight)))
                {
                    var inventoryItemWeight = (ProductByWeight)inventoryItem;
                    var itemToDeleteWeight = (ProductByWeight)itemToDelete;
                    inventoryItemWeight.Weight += itemToDeleteWeight.Weight;
                }
                else
                {
                    var inventoryItemQuantity = (ProductByQuantity)inventoryItem;
                    var itemToDeleteQuantity = (ProductByQuantity)itemToDelete;
                    inventoryItemQuantity.Quantity += itemToDeleteQuantity.Quantity;
                }

            }

            if (itemToDelete is ProductByQuantity)
                FakeDatabase.ProductsByQuantity.Remove((ProductByQuantity)(itemToDelete ?? new ProductByQuantity()));
            else if (itemToDelete is ProductByWeight)
                FakeDatabase.ProductsByWeight.Remove((ProductByWeight)(itemToDelete ?? new ProductByWeight()));

            return id;
            //var paramList = new DeleteParams(id, searchIn, cartName);
            //var response = new WebRequestHandler().Get($"http://loclahost:5013/Product/Delete/{paramList}");

            //var productToDelete = inventory.FirstOrDefault(t => t.Id == id);
            //if (productToDelete == null)
            //{
            //    return;
            //}
            //inventory.Remove(productToDelete);
        }

    }
}
