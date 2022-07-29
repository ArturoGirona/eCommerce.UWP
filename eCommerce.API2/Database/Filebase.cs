using Library.eCommerce.Models;
using Newtonsoft.Json;

namespace eCommerce.API.Database
{
    public class Filebase
    {
        private string _root;
        private string _productByQuantityRoot;
        private string _productByWeightRoot;
        private string _cartRoot;
        private static Filebase _instance;
        private Filebase database = Filebase.Current;


        public static Filebase Current
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Filebase();
                }

                return _instance;
            }
        }

        private Filebase()
        {
            _root = "C:\\temp";
            _productByQuantityRoot = $"{_root}\\ProductByQuantity";
            _productByWeightRoot = $"{_root}\\ProductByWeight";
            _cartRoot = $"{_root}\\Carts";
        }

        //public Product AddOrUpdate(Product product)
        //{

        //    // set up a new Id if one dooesn't already exist
        //    if (product.Id <= 0)
        //    {
        //        // get a new Id
        //        product.Id = NextId;
        //    }

        //    // go to the right place
        //    string path;
        //    if (product is ProductByQuantity)
        //    {
        //        path = $"{_productByQuantityRoot}/{product.Id}.json";
        //    }
        //    else
        //    {
        //        path = $"{_productByWeightRoot}/{product.Id}.json";
        //    }

        //    // if the item has been previously persisted
        //    if (File.Exists(path))
        //    {
        //        // blow it up
        //        File.Delete(path);
        //    }

        //    // write the file
        //    File.WriteAllText(path, JsonConvert.SerializeObject(product));

        //    // return the item, which now has an id
        //    return product;
        //}

        public Product AddOrUpdate(Product item, ProductType searchIn = ProductType.INVENTORY, string cartName = "")
        {
            //Id management for adding a new record.
            if (item.Id == 0)
            {
                if (database.Inventory.Any() && item.FoundIn == ProductType.INVENTORY)
                {
                    item.Id = database.Inventory.Select(i => i.Id).Max() + 1;
                }
                else
                {
                    if (item.FoundIn == ProductType.INVENTORY)
                        item.Id = 1;
                }
            }

            string path;
            if (item is ProductByQuantity)
            {
                path = $"{_productByQuantityRoot}/{item.Id}.json";
            }
            else
            {
                path = $"{_productByWeightRoot}/{item.Id}.json";
            }

            if (!database.Inventory.Any(i => i.Id == item.Id))
            {
                //database.Inventory.Add(item);

                if (item is ProductByQuantity)
                    database.ProductsByQuantity.Add(item as ProductByQuantity);
                else
                    database.ProductsByWeight.Add(item as ProductByWeight);
            }
            else if (database.Inventory.Any(i => i.Id == item.Id && searchIn == ProductType.INVENTORY && i.FoundIn == ProductType.INVENTORY))
            {
                var inventoryItem = database.Inventory.FirstOrDefault(i => i.Id == item.Id && i.FoundIn == ProductType.INVENTORY);

                if (item is ProductByWeight && inventoryItem is ProductByWeight)
                //if (item.GetType().Equals(typeof(ProductByWeight)) && inventoryItem.GetType().Equals(typeof(ProductByWeight)))
                {
                    var invItemWeight = (ProductByWeight)inventoryItem;
                    var itemWeight = (ProductByWeight)item;
                    invItemWeight.Weight = itemWeight.Weight;
                }
                //else if (item.GetType().Equals(typeof(ProductByQuantity)) && inventoryItem.GetType().Equals(typeof(ProductByQuantity)))
                else if (item is ProductByQuantity && inventoryItem is ProductByQuantity)
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
            else if (database.Inventory.Any(i => i.Id == item.Id && searchIn == ProductType.CART && i.FoundIn == ProductType.INVENTORY && cartName != ""))
            {
                var inventoryItem = database.Inventory.FirstOrDefault(i => i.Id == item.Id && i.FoundIn == ProductType.INVENTORY);
                var cartItem = database.Inventory.FirstOrDefault(i => i.Id == item.Id && i.FoundIn == ProductType.CART && i.CartName == cartName);

                if (cartItem != null)
                {

                    //if (inventoryItem.GetType().Equals(typeof(ProductByWeight)))
                    if (inventoryItem is ProductByWeight)
                    {
                        var invItemWeight = (ProductByWeight)inventoryItem;
                        var itemWeight = (ProductByWeight)item;
                        var cartItemWeight = (ProductByWeight)cartItem;
                        invItemWeight.Weight += cartItemWeight.Weight;

                        cartItemWeight.Weight = itemWeight.Weight;
                        invItemWeight.Weight -= cartItemWeight.Weight;
                    }
                    //else if (inventoryItem.GetType().Equals(typeof(ProductByQuantity)))
                    else if (inventoryItem is ProductByWeight)
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
                    //if (inventoryItem.GetType().Equals(typeof(ProductByWeight)))
                    if (inventoryItem is ProductByWeight)
                    {
                        var invItemWeight = (ProductByWeight)inventoryItem;
                        var itemWeight = (ProductByWeight)item;

                        invItemWeight.Weight -= itemWeight.Weight;
                        database.ProductsByWeight.Add(itemWeight);
                    }
                    //else if (inventoryItem.GetType().Equals(typeof(ProductByQuantity)))
                    else if (inventoryItem is ProductByQuantity)
                    {
                        var invItemQuantity = (ProductByQuantity)inventoryItem;
                        var itemQuantity = (ProductByQuantity)item;

                        invItemQuantity.Quantity -= itemQuantity.Quantity;
                        database.ProductsByQuantity.Add(itemQuantity);
                    }
                }
            }
            else
            {
                Console.WriteLine("Sorry, item not found.");
            }

            // if the item has been previously persisted
            if (File.Exists(path))
            {
                // blow it up
                File.Delete(path);
            }

            // write the file
            File.WriteAllText(path, JsonConvert.SerializeObject(item));

            // return the item, which now has an id
            return item;
        }

        //public int Delete(DeleteParams pList)
        public int Delete(int id, ProductType searchIn = ProductType.INVENTORY, string cartName = "")
        {
            //int id = pList.id;
            //ProductType searchIn = pList.searchIn;
            //string cartName = pList.cartName;

            var itemToDelete = database.Inventory.FirstOrDefault(i => i.Id == id && i.FoundIn == searchIn && i.CartName == cartName);
            if (itemToDelete == default || itemToDelete == null)
            {
                Console.WriteLine("The item has not been found and cannot be deleted");
                return id;
            }

            if (searchIn == ProductType.INVENTORY)
            {
                var deleteFromCart = database.Inventory.FirstOrDefault(i => i.Id == id && i.FoundIn == ProductType.CART);
                database.Inventory.Remove(deleteFromCart ?? new ProductByQuantity());
            }
            else
            {
                var inventoryItem = database.Inventory.FirstOrDefault(i => i.Id == id && i.FoundIn == ProductType.INVENTORY);
                //if (itemToDelete.GetType().Equals(typeof(ProductByWeight)))
                if (itemToDelete is ProductByWeight)
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

            //var itemToDelete = database.Inventory.FirstOrDefault(i => i.Id == id);
            //if (itemToDelete != null)
            //{
            //    var item = itemToDelete as ProductByQuantity;
            //    if (item != null)
            //    {
            //        database.ProductsByQuantity.Remove(item);
            //    }
            //}

            return id;
        }


        public List<ProductByQuantity> ProductsByQuantity
        {
            get
            {
                var root = new DirectoryInfo(_productByQuantityRoot);
                var _productsByQuantity = new List<ProductByQuantity>();
                foreach (var productByQuantityFile in root.GetFiles())
                {
                    var productByQuantity = JsonConvert.DeserializeObject<ProductByQuantity>(File.ReadAllText(productByQuantityFile.FullName));
                    _productsByQuantity.Add(productByQuantity);
                }

                return _productsByQuantity;
            }
        }

        public List<string> CartList
        {
            get
            {
                var root = new DirectoryInfo(_cartRoot);
                var _carts = new List<string>();
                foreach (var cartFile in root.GetFiles())
                {
                    var cart = JsonConvert.DeserializeObject<string>(File.ReadAllText(cartFile.FullName));
                    _carts.Add(cart);
                }

                return _carts;
            }
        }
        public List<ProductByWeight> ProductsByWeight
        {
            get
            {
                var root = new DirectoryInfo(_productByWeightRoot);
                var _productsByWeight = new List<ProductByWeight>();
                foreach (var productByWeightFile in root.GetFiles())
                {
                    var productByWeight = JsonConvert.DeserializeObject<ProductByWeight>(File.ReadAllText(productByWeightFile.FullName));
                    _productsByWeight.Add(productByWeight);
                }

                return _productsByWeight;
            }
        }

        public List<Product> Inventory
        {
            get
            {
                var returnList = new List<Product>();
                ProductsByQuantity.ForEach(returnList.Add);
                ProductsByWeight.ForEach(returnList.Add);

                return returnList;
            }
        }

        public bool Delete(string type, string id)
        {
            //TODO: refer to AddOrUpdate for an idea of how you can implement this.
            return true;
        }

        public int NextId
        {
            get
            {
                if (!Inventory.Any())
                {
                    return 1;
                }

                return Inventory.Select(t => t.Id).Max() + 1;
            }
        }
    }
}
