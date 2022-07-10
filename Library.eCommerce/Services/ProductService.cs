using Library.eCommerce.Models;
using Library.TaskManagement.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Library.eCommerce.Services
{
    public class ProductService
    {
        private string _persistPath
                    = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\SaveData.json";
        
        private static ProductService instance;
        public enum ProductBy { DEFAULT, QUANTITY, WEIGHT };

        public static ProductService Current
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductService();
                }

                return instance;
            }
        }

        public IEnumerable<Product> GetFilteredList(string query)
        {

            if(string.IsNullOrEmpty(query))
            {
                return Inventory;
            }

            return Inventory.Where(i =>
                    (i?.Name?.ToUpper()?.Contains(query.ToUpper()) ?? false)
                    || (i?.Name?.ToUpper()?.Contains(query.ToUpper()) ?? false));
        }

        public static explicit operator ObservableCollection<object>(ProductService v)
        {
            throw new NotImplementedException();
        }

        private List<Product> inventory;
        private ListNavigator<Product> listNavigator;
        public List<Product> Inventory
        {
            get
            {
                return inventory;
            }
        }
        
        private ProductService()
        {
            _query = String.Empty;
            inventory = new List<Product>();
            listNavigator = new ListNavigator<Product>(ProcessedList);
        }
        public Dictionary<int, Product> GoForward()
        {
            return listNavigator.GoForward();
        }

        public Dictionary<int, Product> GoBackward()
        {
            return listNavigator.GoBackward();
        }

        public Dictionary<int, Product> GoToPage(int page)
        {
            return listNavigator.GoToPage(page);
        }

        public Dictionary<int, Product> GetCurrentPage()
        {
            return listNavigator.GetCurrentPage();
        }

        public Dictionary<int, Product> GoToFirstPage()
        {
            return listNavigator.GoToFirstPage();
        }

        public Dictionary<int, Product> GoToLastPage()
        {
            return listNavigator.GoToLastPage();
        }

        public int GetPageNumber()
        {
            return listNavigator.PageNumber;
        }

        public int GetLastPageNumber()
        {
            return listNavigator.LastPage;
        }

        public void AddOrUpdate(Product item, ProductType searchIn = ProductType.INVENTORY)
        {
            //Id management for adding a new record.
            if(item.Id == 0)
            {
                if (inventory.Any() && item.FoundIn == ProductType.INVENTORY)
                {
                    item.Id = inventory.Select(i => i.Id).Max() + 1;
                } else
                {
                    if (item.FoundIn == ProductType.INVENTORY)
                        item.Id = 1;
                }
            }

            if (!inventory.Any(i => i.Id == item.Id))
            {
                inventory.Add(item);
            }
            else if (inventory.Any(i => i.Id == item.Id && searchIn == ProductType.INVENTORY && i.FoundIn == ProductType.INVENTORY))
            {
                var inventoryItem = inventory.FirstOrDefault(i => i.Id == item.Id && i.FoundIn == ProductType.INVENTORY);

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
            else if (inventory.Any(i => i.Id == item.Id && searchIn == ProductType.CART && i.FoundIn == ProductType.INVENTORY))
            {
                var inventoryItem = inventory.FirstOrDefault(i => i.Id == item.Id && i.FoundIn == ProductType.INVENTORY);
                var cartItem = inventory.FirstOrDefault(i => i.Id == item.Id && i.FoundIn == ProductType.CART);

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
                        inventory.Add(itemWeight);
                    }
                    else if (inventoryItem.GetType().Equals(typeof(ProductByQuantity)))
                    {
                        var invItemQuantity = (ProductByQuantity)inventoryItem;
                        var itemQuantity = (ProductByQuantity)item;

                        invItemQuantity.Quantity -= itemQuantity.Quantity;
                        inventory.Add(itemQuantity);
                    }
                }
            }
            else
            {
                Console.WriteLine("Sorry, item not found.");
            }
        }

        public void Delete(int id, ProductType searchIn = ProductType.INVENTORY)
        {
            var itemToDelete = inventory.FirstOrDefault(i => i.Id == id && i.FoundIn == searchIn);
            if(itemToDelete == default || itemToDelete == null)
            {
                Console.WriteLine("The item has not been found and cannot be deleted");
                return;
            }


            if (searchIn == ProductType.INVENTORY)
            {
                var deleteFromCart = inventory.FirstOrDefault(i => i.Id == id && i.FoundIn == ProductType.CART);
                inventory.Remove(deleteFromCart ?? new Product());
            }
            else
            {
                var inventoryItem = inventory.FirstOrDefault(i => i.Id == id && i.FoundIn == ProductType.INVENTORY);
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

            inventory.Remove(itemToDelete);
        }
        public void Save()
        {
            var payload = JsonConvert.SerializeObject(inventory, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            File.WriteAllText(_persistPath, payload);
        }

        public void Load()
        {
            if(!string.IsNullOrEmpty(_persistPath) && File.Exists(_persistPath))
            {
                var payload = File.ReadAllText(_persistPath);
                if (!string.IsNullOrEmpty(payload))
                {
                    inventory = JsonConvert.DeserializeObject<List<Product>>(payload, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    }) ?? new List<Product>();
                }

                listNavigator = new ListNavigator<Product>(ProcessedList);
            }
        }

        //GROSS
        //public IEnumerable<Product> Search(string query)
        //{
        //    return Inventory.Where(i => i.Description.Contains(query) || i.Name.Contains(query));
        //}

        //Stateful implementation


        public enum SortType { NAME, PRICE, TOTALPRICE };
        //public enum SortThrough { ALL, INVENTORY, CART };
        private string _query = String.Empty;
        //private SortThrough _sortThrough = SortThrough.ALL;
        private SortType _sortBy = SortType.NAME;
        private ProductType _sortThrough = ProductType.INVENTORY;


        public IEnumerable<Product> Search(string query, SortType element = SortType.NAME, ProductType searchIn = ProductType.INVENTORY)
        {
            _query = query;
            _sortThrough = searchIn;
            listNavigator = new ListNavigator<Product>(ProcessedList);
            return ProcessedList;
        }

        public IEnumerable<Product> ProcessedList
        {
            get
            {
                if(string.IsNullOrEmpty(_query))
                {
                    return Inventory.Where(i => i.FoundIn == _sortThrough);
                }

                var processedlist = Inventory
                    .Where(i =>
                    ((i?.Name?.ToUpper()?.Contains(_query.ToUpper()) ?? false)
                    || (i?.Description?.ToUpper()?.Contains(_query.ToUpper()) ?? false))
                    && i.FoundIn == _sortThrough);

                if (_sortBy == SortType.PRICE)
                {
                    processedlist = processedlist.OrderBy(i => i.Price).ThenBy(i => i.Name);
                }
                else if (_sortBy == SortType.TOTALPRICE)
                {
                    processedlist = processedlist.OrderBy(i => i.TotalPrice).ThenBy(i => i.Name);
                }
                else
                {
                    processedlist = processedlist.OrderBy(i => i.Name);
                }

                return processedlist;
            }
        }
    }
}
