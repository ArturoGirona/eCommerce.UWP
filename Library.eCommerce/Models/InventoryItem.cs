using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.eCommerce.Models
{
    public class InventoryItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int Id { get; set; }

        public override string ToString()
        {
            return $"{Id} ({Quantity}) {Price} {Name}: {Description}";
        }

    }
}