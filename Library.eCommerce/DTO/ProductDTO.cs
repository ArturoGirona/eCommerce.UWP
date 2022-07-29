using Library.eCommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.eCommerce.DTO
{
    public class ProductDTO
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
        //private ProductByQuantity p;

        public ProductDTO()
        {

        }
        public ProductDTO(Product p)
        {
            Id = p.Id;
            Name = p.Name;
            Description = p.Description;
            Price = p.Price;
            BoGo = p.BoGo;
            TotalPrice = p.TotalPrice;
            FoundIn = p.FoundIn;
        }

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

        //public ProductDTO copy()
        //{
        //    Product copy = new Product();

        //    copy.FoundIn = FoundIn;
        //    copy.Name = Name;
        //    copy.Description = Description;
        //    copy.Price = Price;
        //    //copy.Quantity = Quantity;
        //    copy.Id = Id;
        //    copy.CartName = CartName;

        //    return copy;
        //}

    }
}
