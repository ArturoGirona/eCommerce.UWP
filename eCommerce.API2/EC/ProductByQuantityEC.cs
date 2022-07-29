using eCommerce.API.Database;
using Library.eCommerce.Models;
//using Library.eCommerce.DTO;

namespace eCommerce.API.EC
{
    public class ProductByQuantityEC
    {
        public List<ProductByQuantity> Get()
        {
            return FakeDatabase.ProductsByQuantity;//.Select(t => new ProductByQuantity(t));
        }
    }
}
