using Library.eCommerce.Models;
using Library.eCommerce.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.eCommerce.DTO
{

    internal class ProductByQuantityTDO : ProductDTO
    {

        private int _quantity;
        public int Quantity
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
                    return Price * Math.Round((decimal)Quantity / 2, MidpointRounding.AwayFromZero);
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

        //public ProductByQuantity copy()
        //{
        //    ProductByQuantity copy = (base.copy() as ProductByQuantity);

        //    copy.Quantity = Quantity;
        //    return copy;

        //}
    }
}
