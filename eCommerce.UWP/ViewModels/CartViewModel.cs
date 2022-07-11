using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.UWP.ViewModels
{
    public class CartViewModel
    {
        public CartViewModel()
        {

        }

        public CartViewModel(string cvm)
        {
            CartName = cvm;
        }

        public string CartName { get; set; }
    }
}
