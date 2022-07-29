using Library.eCommerce.Models;
using Library.eCommerce.Services;
using eCommerce.API.EC;
using Microsoft.AspNetCore.Mvc;
using eCommerce.API.Database;
//using Library.eCommerce.DTO;

namespace eCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductByQuantityController : ControllerBase
    {
        private readonly ILogger<ProductByQuantityController> _logger;
        public ProductByQuantityController(ILogger<ProductByQuantityController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<ProductByQuantity> Get()
        {
            return new ProductByQuantityEC().Get();
        }

        [HttpPost("AddOrUpdate")]
        public ProductByQuantity AddOrUpdate(AddOrUpdateParams pList)
        //public ProductByWeight AddOrUpdate(ProductByWeight item, ProductType searchIn = ProductType.INVENTORY, string cartName = "")
        {
            ProductByQuantity item = pList.item as ProductByQuantity;
            ProductType searchIn = pList.searchIn;
            string cartName = pList.cartName;

            //return (ProductByQuantity) Filebase.Current.AddOrUpdate(item, searchIn, cartName);
            ////Id management for adding a new record.
            //if (item.Id == 0)
            //{
            //    if (FakeDatabase.ProductsByQuantity.Any() && item.FoundIn == ProductType.INVENTORY)
            //    {
            //        item.Id = FakeDatabase.ProductsByQuantity.Select(i => i.Id).Max() + 1;
            //    }
            //    else
            //    {
            //        if (item.FoundIn == ProductType.INVENTORY)
            //            item.Id = 1;
            //    }
            //}

            if (!FakeDatabase.ProductsByQuantity.Any(i => i.Id == item.Id))
            {
                FakeDatabase.ProductsByQuantity.Add(item);
                //return item;
            }
            else if (FakeDatabase.ProductsByQuantity.Any(i => i.Id == item.Id && searchIn == ProductType.INVENTORY && i.FoundIn == ProductType.INVENTORY))
            {
                var inventoryItem = FakeDatabase.ProductsByQuantity.FirstOrDefault(i => i.Id == item.Id && i.FoundIn == ProductType.INVENTORY);

                //if (item.GetType().Equals(typeof(ProductByWeight)) && inventoryItem.GetType().Equals(typeof(ProductByWeight)))
                //{
                //    var invItemWeight = (ProductByWeight)inventoryItem;
                //    var itemWeight = (ProductByWeight)item;
                //    invItemWeight.Weight = itemWeight.Weight;
                //}
                //else if (item.GetType().Equals(typeof(ProductByQuantity)) && inventoryItem.GetType().Equals(typeof(ProductByQuantity)))
                //{
                var invItemQuantity = (ProductByQuantity)inventoryItem;
                var itemQuantity = (ProductByQuantity)item;
                invItemQuantity.Quantity = itemQuantity.Quantity;
                //return itemQuantity;
                //}
                //else
                //{
                //    Console.WriteLine("Sorry, an error has occurred");
                //}
            }
            else if (FakeDatabase.ProductsByQuantity.Any(i => i.Id == item.Id && searchIn == ProductType.CART && i.FoundIn == ProductType.INVENTORY && cartName != ""))
            {
                var inventoryItem = FakeDatabase.ProductsByQuantity.FirstOrDefault(i => i.Id == item.Id && i.FoundIn == ProductType.INVENTORY);
                var cartItem = FakeDatabase.ProductsByQuantity.FirstOrDefault(i => i.Id == item.Id && i.FoundIn == ProductType.CART && i.CartName == cartName);

                if (cartItem != null)
                {

                    //if (inventoryItem.GetType().Equals(typeof(ProductByWeight)))
                    //{
                    //    var invItemWeight = (ProductByWeight)inventoryItem;
                    //    var itemWeight = (ProductByWeight)item;
                    //    var cartItemWeight = (ProductByWeight)cartItem;
                    //    invItemWeight.Weight += cartItemWeight.Weight;

                    //    cartItemWeight.Weight = itemWeight.Weight;
                    //    invItemWeight.Weight -= cartItemWeight.Weight;
                    //}
                    //else if (inventoryItem.GetType().Equals(typeof(ProductByQuantity)))
                    //{
                    var invItemQuantity = (ProductByQuantity)inventoryItem;
                    var itemQuantity = (ProductByQuantity)item;
                    var cartItemQuantity = (ProductByQuantity)cartItem;
                    invItemQuantity.Quantity += cartItemQuantity.Quantity;

                    cartItemQuantity.Quantity = itemQuantity.Quantity;
                    invItemQuantity.Quantity -= cartItemQuantity.Quantity;
                    //return cartItemQuantity;
                    //}
                }
                else
                {
                    //if (inventoryItem.GetType().Equals(typeof(ProductByWeight)))
                    //{
                    //    var invItemWeight = (ProductByWeight)inventoryItem;
                    //    var itemWeight = (ProductByWeight)item;

                    //    invItemWeight.Weight -= itemWeight.Weight;
                    //    inventory.Add(itemWeight);
                    //}
                    //else if (inventoryItem.GetType().Equals(typeof(ProductByQuantity)))
                    //{
                    var invItemQuantity = (ProductByQuantity)inventoryItem;
                    var itemQuantity = (ProductByQuantity)item;

                    invItemQuantity.Quantity -= itemQuantity.Quantity;
                    FakeDatabase.ProductsByQuantity.Add(itemQuantity);
                    //return itemQuantity;
                    //}
                }
            }
            else
            {
                Console.WriteLine("Sorry, item not found.");
                //return item;
            }

            return item;
        }
    }
}
