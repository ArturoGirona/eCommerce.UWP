using eCommerce.API.Database;
using Library.eCommerce.Models;

namespace eCommerce.API.EC
{
    public class ProductByQuantityEC
    {
        public List<ProductByQuantity> Get()
        {
            return FakeDatabase.ProductsByQuantity;
        }
    }
}
