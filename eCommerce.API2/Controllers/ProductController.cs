using Library.eCommerce.Models;
using Microsoft.AspNetCore.Mvc;
using eCommerce.API.Database;
using Library.eCommerce.Services;

namespace eCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<Product> Get()
        {
            return FakeDatabase.Inventory ;
        }

        [HttpPost("AddOrUpdate")]
        public Product AddOrUpdate(Product item, ProductType searchIn = ProductType.INVENTORY, string cartName = "")
        {
            //Id management for adding a new record.
            if (item.Id == 0)
            {
                if (FakeDatabase.Inventory.Any() && item.FoundIn == ProductType.INVENTORY)
                {
                    item.Id = FakeDatabase.Inventory.Select(i => i.Id).Max() + 1;
                }
                else
                {
                    if (item.FoundIn == ProductType.INVENTORY)
                        item.Id = 1;
                }
            }

            if (!FakeDatabase.Inventory.Any(i => i.Id == item.Id))
            {
                FakeDatabase.Inventory.Add(item);
            }
            else if (FakeDatabase.Inventory.Any(i => i.Id == item.Id && searchIn == ProductType.INVENTORY && i.FoundIn == ProductType.INVENTORY))
            {
                var inventoryItem = FakeDatabase.Inventory.FirstOrDefault(i => i.Id == item.Id && i.FoundIn == ProductType.INVENTORY);

                if (item.GetType().Equals(typeof(ProductByWeight)) && inventoryItem.GetType().Equals(typeof(ProductByWeight)))
                {
                    var invItemWeight = (ProductByWeight)inventoryItem;
                    var itemWeight = (ProductByWeight)item;
                    invItemWeight.Weight = itemWeight.Weight;
                }
                else if (item.GetType().Equals(typeof(ProductByQuantity)) && inventoryItem.GetType().Equals(typeof(ProductByQuantity)))
                {
                    var invItemQuantity = (ProductByQuantity)inventoryItem;
                    var itemQuantity = (ProductByQuantity)item;
                    invItemQuantity.Quantity = itemQuantity.Quantity;
                }
                else
                {
                    Console.WriteLine("Sorry, an error has occurred");
                }
            }
            else if (FakeDatabase.Inventory.Any(i => i.Id == item.Id && searchIn == ProductType.CART && i.FoundIn == ProductType.INVENTORY && cartName != ""))
            {
                var inventoryItem = FakeDatabase.Inventory.FirstOrDefault(i => i.Id == item.Id && i.FoundIn == ProductType.INVENTORY);
                var cartItem = FakeDatabase.Inventory.FirstOrDefault(i => i.Id == item.Id && i.FoundIn == ProductType.CART && i.CartName == cartName);

                if (cartItem != null)
                {

                    if (inventoryItem.GetType().Equals(typeof(ProductByWeight)))
                    {
                        var invItemWeight = (ProductByWeight)inventoryItem;
                        var itemWeight = (ProductByWeight)item;
                        var cartItemWeight = (ProductByWeight)cartItem;
                        invItemWeight.Weight += cartItemWeight.Weight;

                        cartItemWeight.Weight = itemWeight.Weight;
                        invItemWeight.Weight -= cartItemWeight.Weight;
                    }
                    else if (inventoryItem.GetType().Equals(typeof(ProductByQuantity)))
                    {
                        var invItemQuantity = (ProductByQuantity)inventoryItem;
                        var itemQuantity = (ProductByQuantity)item;
                        var cartItemQuantity = (ProductByQuantity)cartItem;
                        invItemQuantity.Quantity += cartItemQuantity.Quantity;

                        cartItemQuantity.Quantity = itemQuantity.Quantity;
                        invItemQuantity.Quantity -= cartItemQuantity.Quantity;
                    }
                }
                else
                {
                    if (inventoryItem.GetType().Equals(typeof(ProductByWeight)))
                    {
                        var invItemWeight = (ProductByWeight)inventoryItem;
                        var itemWeight = (ProductByWeight)item;

                        invItemWeight.Weight -= itemWeight.Weight;
                        FakeDatabase.Inventory.Add(itemWeight);
                    }
                    else if (inventoryItem.GetType().Equals(typeof(ProductByQuantity)))
                    {
                        var invItemQuantity = (ProductByQuantity)inventoryItem;
                        var itemQuantity = (ProductByQuantity)item;

                        invItemQuantity.Quantity -= itemQuantity.Quantity;
                        FakeDatabase.Inventory.Add(itemQuantity);
                    }
                }
            }
            else
            {
                Console.WriteLine("Sorry, item not found.");
            }

            return item;
        }

        [HttpPost("Delete/{pList}")]
        public int Delete(DeleteParams pList)
        //public int Delete(int id, ProductType searchIn = ProductType.INVENTORY, string cartName = "")
        {
            int id = pList.id;
            ProductType searchIn = pList.searchIn;
            string cartName = pList.cartName;

            var itemToDelete = FakeDatabase.Inventory.FirstOrDefault(i => i.Id == id && i.FoundIn == searchIn && i.CartName == cartName);
            if (itemToDelete == default || itemToDelete == null)
            {
                Console.WriteLine("The item has not been found and cannot be deleted");
                return id;
            }


            if (searchIn == ProductType.INVENTORY)
            {
                var deleteFromCart = FakeDatabase.Inventory.FirstOrDefault(i => i.Id == id && i.FoundIn == ProductType.CART);
                FakeDatabase.Inventory.Remove(deleteFromCart ?? new Product());
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

            FakeDatabase.Inventory.Remove(itemToDelete);

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
