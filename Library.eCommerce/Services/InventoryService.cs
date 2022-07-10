using Library.eCommerce.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.eCommerce.Services
{
    public class InventoryService
    {
        private string _persistPath = "inventory.json";
        private static InventoryService instance;
        public static InventoryService Current
        {
            get
            {
                if (instance == null)
                {
                    instance = new InventoryService();
                }

                return instance;
            }
        }

        public IEnumerable<InventoryItem> GetFilteredList(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return Inventory;
            }
            return Inventory.Where(i =>
                    (i?.Name?.ToUpper()?.Contains(query.ToUpper()) ?? false)
                    || (i?.Description?.ToUpper()?.Contains(query.ToUpper()) ?? false));
        }

        private List<InventoryItem> inventory;
        public List<InventoryItem> Inventory
        {
            get { return inventory; }
        }
        private InventoryService()
        {
            inventory = new List<InventoryItem>();
        }

        public void AddOrUpdate(InventoryItem item)
        {
            //Id management for adding a new record.
            if (item.Id == 0)
            {
                if (inventory.Any())
                {
                    item.Id = inventory.Select(i => i.Id).Max() + 1;
                }
                else
                {
                    item.Id = 1;
                }
            }

            if (!inventory.Any(i => i.Id == item.Id))
            {
                inventory.Add(item);
            }
        }

        public void Delete(int id)
        {
            var itemToDelete = inventory.FirstOrDefault(i => i.Id == id);
            if (itemToDelete != null)
            {
                inventory.Remove(itemToDelete);
            }
        }


        public void Save()
        {
            var payload = JsonConvert.SerializeObject(inventory);
            File.WriteAllText(_persistPath, payload);
        }

        public void Load()
        {
            var payload = File.ReadAllText(_persistPath);
            if (!string.IsNullOrEmpty(payload))
            {
                inventory = JsonConvert.DeserializeObject<List<InventoryItem>>(payload) ?? new List<InventoryItem>();
            }
        }
    }
}