using eCommerce.UWP.Dialogs;
using Library.eCommerce.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static Library.eCommerce.Services.ProductService;

namespace eCommerce.UWP.ViewModels
{
    public class ProductViewModel:INotifyPropertyChanged
    {
        public Product BoundProduct
        {
            get
            {
                if (BoundProductByQuantity != null)
                {
                    //var bpq = BoundProductByQuantity;
                    return BoundProductByQuantity;
                }

                return BoundProductByWeight;
            }
        }

        public ProductViewModel()
        {
            //QuantityNotWeight = true;
            boundProductByQuantity = new ProductByQuantity();
            boundProductByWeight = null;
        }

        public ProductViewModel(ProductViewModel p)
        {
            if (p.QuantityNotWeight())
            {
                boundProductByQuantity = p.BoundProductByQuantity;
                boundProductByWeight = null;
            }
            else if (p.WeightNotQuantity())
            {
                boundProductByWeight = p.BoundProductByWeight;
                boundProductByQuantity = null;
            }
        }

        public bool QuantityNotWeight()
        {
            return (boundProductByQuantity != null && boundProductByWeight == null);
        }

        public bool WeightNotQuantity()
        {
            return (boundProductByQuantity == null && boundProductByWeight != null);
        }
        public string Name
        {
            //get => BoundProductByQuantity?.Name ?? BoundProductByWeight?.Name ?? String.Empty;

            //set
            //{
            //    if (QuantityNotWeight())
            //        BoundProductByQuantity.Name = value;
            //    else if (WeightNotQuantity())
            //        BoundProductByWeight.Name = value;
            //    else
            //        return;
            //}

            get => BoundProduct?.Name ?? String.Empty;

            set
            {
                if (BoundProduct == null)
                    return;

                BoundProduct.Name = value;
            }
        }

        public string Description
        {
            //get => BoundProductByQuantity?.Description ?? BoundProductByWeight?.Description ?? String.Empty;

            //set
            //{
            //    if (QuantityNotWeight())
            //        BoundProductByQuantity.Description = value;
            //    else if (WeightNotQuantity())
            //        BoundProductByWeight.Description = value;
            //    else
            //        return;
            //}

            get => BoundProduct?.Description ?? string.Empty;

            set
            {
                if (BoundProduct == null)
                    return;

                BoundProduct.Description = value;
            }
        }

        public ProductType FoundIn
        {
            get
            {
                return ProductType.INVENTORY;
            }
        }

        public int Id
        {
            get
            {
                return BoundProduct?.Id ?? 0;
            }
        }

        public double Price
        {
            //get => BoundProductByQuantity?.Price ?? BoundProductByWeight?.Price ?? 0M;

            //set
            //{
            //    if (QuantityNotWeight())
            //        BoundProductByQuantity.Price = value;
            //    else if (WeightNotQuantity())
            //        BoundProductByWeight.Price = value;
            //    else
            //        return;
            //}

            //get => BoundProduct?.Price ?? 0M;

            //set
            //{
            //    if (BoundProduct == null)
            //        return;


            //    BoundProduct.Price = value;
            //}

            get => (double)(BoundProduct?.Price ?? 0);

            set
            {
                BoundProduct.Price = (decimal)value;
            }
        }

        public string PriceString => BoundProduct?.Price.ToString("C");
        private int _quantity;
        public int Quantity
        {
            get => boundProductByQuantity?.Quantity ?? 0;

            set
            {
                if (WeightNotQuantity())
                    return;

                boundProductByQuantity.Quantity = value;
            }
        }

       // public int QuantityString

        public decimal Weight
        {

            get => boundProductByWeight?.Weight ?? 0M;

            set
            {
                if (QuantityNotWeight())
                    return;

                boundProductByWeight.Weight = value;
            }
        }

        public bool BoGo
        {
            get => BoundProduct?.BoGo ?? false;

            set
            {
                //if (QuantityNotWeight())
                //    BoundProductByQuantity.BoGo = value;
                //else if (WeightNotQuantity())
                //    BoundProductByWeight.BoGo = value;
                //else
                //    return;

                if (BoundProduct == null)
                    return;

                BoundProduct.BoGo = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public override string ToString()
        {
            return $"{Id} {Name} :: {Description}";
        }

        public Visibility IsQuantityCardVisible
        {
            get
            {
                return BoundProductByQuantity == null && BoundProductByWeight != null ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public Visibility IsWeightCardVisible
        {
            get
            {
                return BoundProductByWeight == null && BoundProductByQuantity  != null ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public bool IsProductByQuantity
        {
            get
            {
                return BoundProductByQuantity != null;
            }

            set
            {
                if (value)
                {
                    boundProductByQuantity = new ProductByQuantity();
                    boundProductByWeight = null;
                    NotifyPropertyChanged("IsQuantityCardVisible");
                    NotifyPropertyChanged("IsWeightCardVisible");
                }
            }
        }

        public bool IsProductByWeight
        {
            get
            {
                return BoundProductByWeight != null;
            }

            set
            {
                if (value)
                {
                    boundProductByWeight = new ProductByWeight();
                    boundProductByQuantity = null;
                    NotifyPropertyChanged("IsQuantityCardVisible");
                    NotifyPropertyChanged("IsWeightCardVisible");
                }
            }
        }

        private ProductByQuantity boundProductByQuantity;
        public ProductByQuantity BoundProductByQuantity
        {
            get
            {
                return boundProductByQuantity;
            }
        }

        private ProductByWeight boundProductByWeight;
        public ProductByWeight BoundProductByWeight
        {
            get
            {
                return boundProductByWeight;
            }
        }

        public ProductViewModel(Product i)
        {
            if (i == null)
            {
                return;
            }

            if (i is ProductByQuantity)
            {
                boundProductByQuantity = i as ProductByQuantity;
                boundProductByWeight = null;
            }
            else if (i is ProductByQuantity)
            {
                boundProductByWeight = i as ProductByWeight;
                boundProductByQuantity = null;
            }
        }

    }
}
