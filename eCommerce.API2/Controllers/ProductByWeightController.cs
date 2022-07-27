using Library.eCommerce.Models;
using Library.eCommerce.Services;
using Microsoft.AspNetCore.Mvc;
using eCommerce.API.Database;

namespace eCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductByWeightController : ControllerBase
    {
        private readonly ILogger<ProductByWeightController> _logger;
        public ProductByWeightController(ILogger<ProductByWeightController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<ProductByWeight> Get()
        {
            return FakeDatabase.ProductsByWeight;
        }

        [HttpPost("AddOrUpdate")]
        public ProductByWeight AddOrUpdate(AddOrUpdateParams pList)
        //public ProductByWeight AddOrUpdate(ProductByWeight item, ProductType searchIn = ProductType.INVENTORY, string cartName = "")
        {
            ProductByWeight item = pList.item as ProductByWeight;
            ProductType searchIn = pList.searchIn;
            string cartName = pList.cartName;
            ////Id management for adding a new record.
            //if (item.Id == 0)
            //{
            //    if (FakeDatabase.ProductsByWeight.Any() && item.FoundIn == ProductType.INVENTORY)
            //    {
            //        item.Id = FakeDatabase.ProductsByWeight.Select(i => i.Id).Max() + 1;
            //    }
            //    else
            //    {
            //        if (item.FoundIn == ProductType.INVENTORY)
            //            item.Id = 1;
            //    }
            //}

            if (!FakeDatabase.ProductsByWeight.Any(i => i.Id == item.Id))
            {
                FakeDatabase.ProductsByWeight.Add(item);
                //return item;
            }
            else if (FakeDatabase.ProductsByWeight.Any(i => i.Id == item.Id && searchIn == ProductType.INVENTORY && i.FoundIn == ProductType.INVENTORY))
            {
                var inventoryItem = FakeDatabase.ProductsByWeight.FirstOrDefault(i => i.Id == item.Id && i.FoundIn == ProductType.INVENTORY);

                //if (item.GetType().Equals(typeof(ProductByWeight)) && inventoryItem.GetType().Equals(typeof(ProductByWeight)))
                //{
                    var invItemWeight = (ProductByWeight)inventoryItem;
                    var itemWeight = (ProductByWeight)item;
                    invItemWeight.Weight = itemWeight.Weight;
                //return invItemWeight;
                //}
                //else if (item.GetType().Equals(typeof(ProductByQuantity)) && inventoryItem.GetType().Equals(typeof(ProductByQuantity)))
                //{
                //    var invItemQuantity = (ProductByQuantity)inventoryItem;
                //    var itemQuantity = (ProductByQuantity)item;
                //    invItemQuantity.Quantity = itemQuantity.Quantity;
                //}
                //else
                //{
                //    Console.WriteLine("Sorry, an error has occurred");
                //}
            }
            else if (FakeDatabase.ProductsByWeight.Any(i => i.Id == item.Id && searchIn == ProductType.CART && i.FoundIn == ProductType.INVENTORY && cartName != ""))
            {
                var inventoryItem = FakeDatabase.ProductsByWeight.FirstOrDefault(i => i.Id == item.Id && i.FoundIn == ProductType.INVENTORY);
                var cartItem = FakeDatabase.ProductsByWeight.FirstOrDefault(i => i.Id == item.Id && i.FoundIn == ProductType.CART && i.CartName == cartName);

                if (cartItem != null)
                {

                    //if (inventoryItem.GetType().Equals(typeof(ProductByWeight)))
                    //{
                        var invItemWeight = (ProductByWeight)inventoryItem;
                        var itemWeight = (ProductByWeight)item;
                        var cartItemWeight = (ProductByWeight)cartItem;
                        invItemWeight.Weight += cartItemWeight.Weight;

                        cartItemWeight.Weight = itemWeight.Weight;
                        invItemWeight.Weight -= cartItemWeight.Weight;
                    //return cartItemWeight;
                    //}
                    //else if (inventoryItem.GetType().Equals(typeof(ProductByQuantity)))
                    //{
                    //    var invItemQuantity = (ProductByQuantity)inventoryItem;
                    //    var itemQuantity = (ProductByQuantity)item;
                    //    var cartItemQuantity = (ProductByQuantity)cartItem;
                    //    invItemQuantity.Quantity += cartItemQuantity.Quantity;

                    //    cartItemQuantity.Quantity = itemQuantity.Quantity;
                    //    invItemQuantity.Quantity -= cartItemQuantity.Quantity;
                    //}
                }
                else
                {
                    //if (inventoryItem.GetType().Equals(typeof(ProductByWeight)))
                    //{
                        var invItemWeight = (ProductByWeight)inventoryItem;
                        var itemWeight = (ProductByWeight)item;

                        invItemWeight.Weight -= itemWeight.Weight;
                        FakeDatabase.ProductsByWeight.Add(itemWeight);
                    //return invItemWeight;
                    //}
                    //else if (inventoryItem.GetType().Equals(typeof(ProductByQuantity)))
                    //{
                    //    var invItemQuantity = (ProductByQuantity)inventoryItem;
                    //    var itemQuantity = (ProductByQuantity)item;

                    //    invItemQuantity.Quantity -= itemQuantity.Quantity;
                    //    FakeDatabase.ProductsByWeight.Add(itemQuantity);
                    //}
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

            var itemToDelete = FakeDatabase.ProductsByWeight.FirstOrDefault(i => i.Id == id && i.FoundIn == searchIn && i.CartName == cartName);
            if (itemToDelete == default || itemToDelete == null)
            {
                Console.WriteLine("The item has not been found and cannot be deleted");
                return id;
            }


            if (searchIn == ProductType.INVENTORY)
            {
                var deleteFromCart = FakeDatabase.ProductsByWeight.FirstOrDefault(i => i.Id == id && i.FoundIn == ProductType.CART);
                FakeDatabase.ProductsByWeight.Remove(deleteFromCart ?? new ProductByWeight());
            }
            else
            {
                var inventoryItem = FakeDatabase.ProductsByWeight.FirstOrDefault(i => i.Id == id && i.FoundIn == ProductType.INVENTORY);
                //if (itemToDelete.GetType().Equals(typeof(ProductByWeight)))
                //{
                    var inventoryItemWeight = (ProductByWeight)inventoryItem;
                    var itemToDeleteWeight = (ProductByWeight)itemToDelete;
                    inventoryItemWeight.Weight += itemToDeleteWeight.Weight;
                //}
                //else
                //{
                    //var inventoryItemQuantity = (ProductByQuantity)inventoryItem;
                    //var itemToDeleteQuantity = (ProductByQuantity)itemToDelete;
                    //inventoryItemQuantity.Quantity += itemToDeleteQuantity.Quantity;
                //}

            }

            //FakeDatabase.ProductsByWeight.Remove(itemToDelete);

            return id;
        }

    }
}
