using Library.eCommerce.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.eCommerce.Models
{
    [JsonConverter(typeof(ProductJsonConverter))]

    public class ProductByWeight : Product
    {
        private decimal _weight;
        public decimal Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                if (value >= 0)
                {
                    _weight = value;
                }
                else
                {
                    _weight = 0;
                }
            }
        }

        public override decimal TotalPrice
        {
            get
            {
                if (BoGo)
                    return Price * Math.Ceiling(Weight / 2);
                else
                    return Price * Weight;
            }
        }

        public override string ToString()
        {
            if (FoundIn == ProductType.INVENTORY)
                return $"{Id} ({Weight} lbs) {Price:C2}/lb {Name}: {Description}";
            else
                return $"{Id} ({Weight} lbs) {Price:C2}/lb | {TotalPrice:C2} {Name}: {Description}";
        }


        public new ProductByWeight copy()
        {
            ProductByWeight copy = (base.copy() as ProductByWeight);

            copy.Weight = Weight;
            return copy;
        }
    }
}
