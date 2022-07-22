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
//using static Library.eCommerce.Services.ProductService;

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
                    return BoundProductByQuantity;
                }

                return BoundProductByWeight;
            }
        }

        public ProductViewModel()
        {
            boundProductByQuantity = new ProductByQuantity();
            boundProductByWeight = null;
        }

        public ProductViewModel(ProductViewModel p)
        {
            if (p.QuantityNotWeight())
            {
                boundProductByQuantity = p.BoundProductByQuantity;

                boundProductByQuantity.Name = Name;
                boundProductByQuantity.Description = Description;
                boundProductByQuantity.Price = (decimal)Price;

                boundProductByWeight = null;

            }
            else if (p.WeightNotQuantity())
            {
                boundProductByWeight = p.BoundProductByWeight;

                boundProductByWeight.Name = Name;
                boundProductByWeight.Description = Description;
                boundProductByWeight.Price = (decimal)Price;

                boundProductByQuantity = null;
            }
        }

        public ProductViewModel(ProductByQuantity p)
        {

            boundProductByQuantity = new ProductByQuantity();
            boundProductByWeight   = null;

            boundProductByQuantity.Id          = p.Id;
            boundProductByQuantity.Name        = p.Name;
            boundProductByQuantity.Description = p.Description;
            boundProductByQuantity.Price       = p.Price;
            boundProductByQuantity.Quantity    = p.Quantity;
            boundProductByQuantity.FoundIn     = p.FoundIn;
        }

        public ProductViewModel(ProductByWeight p)
        {
            boundProductByWeight   = new ProductByWeight();
            boundProductByQuantity = null;

            boundProductByWeight.Id          = p.Id;
            boundProductByWeight.Name        = p.Name;
            boundProductByWeight.Description = p.Description;
            boundProductByWeight.Price       = p.Price;
            boundProductByWeight.Weight      = p.Weight;
            boundProductByWeight.FoundIn     = p.FoundIn;
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

            get => BoundProduct?.Description ?? string.Empty;

            set
            {
                if (BoundProduct == null)
                    return;

                BoundProduct.Description = value;
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
            get => (double)(BoundProduct?.Price ?? 0);

            set
            {
                BoundProduct.Price = (decimal)value;
            }
        }

        public string PriceString => BoundProduct?.Price.ToString("C");
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

        public double Weight
        {

            get => (double) (boundProductByWeight?.Weight ?? 0);

            set
            {
                if (QuantityNotWeight())
                    return;

                boundProductByWeight.Weight = (decimal) value;
            }
        }

        public ProductType FoundIn
        {
            get; set;
        }

        public bool BoGo
        {
            get => BoundProduct?.BoGo ?? false;

            set
            {
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

        public Visibility IsBogo
        {
            get
            {
                return (bool)(BoGo) ? Visibility.Visible : Visibility.Collapsed;
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

                    boundProductByQuantity.Id = Id;
                    boundProductByQuantity.Name = Name;
                    boundProductByQuantity.Description = Description;
                    boundProductByQuantity.Price = (decimal)Price;
                    boundProductByQuantity.FoundIn = FoundIn;

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

                    boundProductByWeight.Id = Id;
                    boundProductByWeight.Name = Name;
                    boundProductByWeight.Description = Description;
                    boundProductByWeight.Price = (decimal)Price;
                    boundProductByWeight.FoundIn = FoundIn;

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
            else if (i is ProductByWeight)
            {
                boundProductByWeight = i as ProductByWeight;
                boundProductByQuantity = null;
            }
        }

    }
}
