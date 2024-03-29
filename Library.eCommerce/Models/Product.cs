﻿using Library.eCommerce.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.eCommerce.Models
{
    public enum ProductType { INVENTORY, CART };

    [JsonConverter(typeof(ProductJsonConverter))]

    public class Product
    {
        public ProductType FoundIn { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        //public virtual int Quantity { get; set; }

        public virtual decimal TotalPrice { get; set; }

        //public virtual decimal? Weight { get; set; }

        public int Id { get; set; }

        public bool BoGo { get; set; }

        private string _cartName;
        public string CartName
        {

            get
            {
                return _cartName;
            }
            set
            {
                if (FoundIn.Equals(ProductType.INVENTORY))
                    _cartName = string.Empty;
                else
                    _cartName = value;
            }
        }

        public override string ToString()
        {
            return $"{Id} () {Price:C2} {Name}: {Description}";
        }

        public Product copy()
        {
            Product copy = new Product();

            copy.FoundIn = FoundIn;
            copy.Name = Name;
            copy.Description = Description;
            copy.Price = Price;
            //copy.Quantity = Quantity;
            copy.Id = Id;
            copy.CartName = CartName;

            return copy;
        }
    }
}
