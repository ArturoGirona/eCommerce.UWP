using Library.eCommerce.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.eCommerce.Utility
{
    public class ProductJsonConverter : JsonCreationConverter<Product>
    {
        protected override Product Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException("jObject");

            if (jObject["quantity"] != null || jObject["Quantity"] != null)
            {
                return new ProductByQuantity();
            }
            else if (jObject["weight"] != null || jObject["Weight"] != null)
            {
                return new ProductByWeight();
            }
            else
            {
                return new Product();
            }
        }
    }
}
