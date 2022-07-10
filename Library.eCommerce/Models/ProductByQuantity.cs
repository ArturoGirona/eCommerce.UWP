﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.eCommerce.Models
{
    public class ProductByQuantity : Product
    {
        private int _quantity;
        public override int Quantity
        {
            get
            {
                return _quantity;
            }

            set
            {
                if (value >= 0)
                    _quantity = value;
                else
                    _quantity = 0;
            }
        }

        public override decimal TotalPrice
        {
            get
            {
                if (BoGo)
                    return Price * Math.Round((decimal) Quantity / 2, MidpointRounding.AwayFromZero);
                else
                    return Price * Quantity;
            }
        }
        
        public override string ToString()
        {
            if (FoundIn == ProductType.INVENTORY)
                return $"{Id} ({Quantity}) {Price:C2} {Name}: {Description}";
            else
                return $"{Id} ({Quantity}) {Price:C2} | {TotalPrice:C2} {Name}: {Description}";
        }
    }
}