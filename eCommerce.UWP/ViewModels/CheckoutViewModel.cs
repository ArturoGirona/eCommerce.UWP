using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.UWP.ViewModels
{
    public class CheckoutViewModel
    {

        public CheckoutViewModel(eCommerceViewModel ecvm)
        {
            ECVM = ecvm;

            subTotal = 0;

            for (int i = 0; i < ECVM.CartItems.Count; i++)
            {
                subTotal += (double)ECVM.CartItems.ElementAt(i).BoundProduct.TotalPrice;
            }

            total = subTotal * 1.07;
        }

        private eCommerceViewModel ECVM;
        public double subTotal;
        public double total;

        public string SubTotalString
        {
            get
            {
                return "$" + subTotal;
            }
        }

        public string TotalString
        {
            get
            {
                return "$" + total;
            }
        }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public string Name { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CVC { get; set; }
    }
}
