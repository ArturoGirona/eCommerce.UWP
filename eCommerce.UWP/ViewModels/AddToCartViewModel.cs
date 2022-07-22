using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.UWP.ViewModels
{
    public class AddToCartViewModel
    {
        public AddToCartViewModel()
        {

        }

        public int Quantity { get; set; }

        public double Weight { get; set; }

        public decimal WeightData
        {
            get => (decimal)Weight;
        }
    }
}
