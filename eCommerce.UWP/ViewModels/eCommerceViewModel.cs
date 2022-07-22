using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Library.eCommerce.Services;
using Library.eCommerce.Models;
using System.Collections.ObjectModel;
//using static Library.eCommerce.Services.ProductService;
using eCommerce.UWP.Dialogs;
using Windows.UI.Core.Preview;

namespace eCommerce.UWP.ViewModels
{
    public class eCommerceViewModel : INotifyPropertyChanged
    {
        public string Query { get; set; }

        public ProductViewModel SelectedProduct { get; set; }
        //public CartViewModel SelectedCart { get; set; }

        private CartViewModel selectedCart;

        private int _sortMode = 0;
        public CartViewModel SelectedCart
        {
            get
            {
                return selectedCart;
            }

            set
            {
                selectedCart = value;

                // This implementation of SelectedCart is yielding 'operation not supported errors', with or without the calls to NotifyPropertyChanged going on here. Can't tell why.
                //NotifyPropertyChanged("Products");
                NotifyPropertyChanged("CartItems");
            }

            // Don't know why it only happens with this and not SelectedCartItem
        }

        private ProductViewModel _selectedCartItem;
        public ProductViewModel SelectedCartItem
        {

            get
            {
                return _selectedCartItem;
            }

            set
            {
                _selectedCartItem = value;
            }
        }

        private ProductService _productService;

        public eCommerceViewModel()
        {
            _productService = ProductService.Current;
            //_productService.Load();

            //SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += (s, e) =>
            //{
            //    _ = Save();
            //};
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<ProductViewModel> Products
        {
            get
            {
                if (_productService == null)
                {
                    return new ObservableCollection<ProductViewModel>();
                }

                if (string.IsNullOrEmpty(Query))
                {
                    switch (_sortMode)
                    {
                        default:
                        case 0:
                            return new ObservableCollection<ProductViewModel>(_productService.Inventory.Where(i => i.FoundIn == ProductType.INVENTORY).OrderBy(i => i.Id).Select(i => new ProductViewModel(i)));
                        case 1:
                            return new ObservableCollection<ProductViewModel>(_productService.Inventory.Where(i => i.FoundIn == ProductType.INVENTORY).OrderBy(i => i.Name).Select(i => new ProductViewModel(i)));
                        case 2:
                            return new ObservableCollection<ProductViewModel>(_productService.Inventory.Where(i => i.FoundIn == ProductType.INVENTORY).OrderBy(i => i.Price).Select(i => new ProductViewModel(i)));
                    }
                    //return new ObservableCollection<ProductViewModel>(_productService.Inventory.Select(i => new ProductViewModel(i)));
                }
                else
                {
                    switch (_sortMode)
                    {
                        default:
                        case 0:
                            return new ObservableCollection<ProductViewModel>(
                                _productService.Inventory.Where(i => i.FoundIn == ProductType.INVENTORY && (i.Name.ToUpper().Contains(Query.ToUpper())
                                    || i.Description.ToUpper().Contains(Query.ToUpper()))).OrderBy(i => i.Id)
                                .Select(i => new ProductViewModel(i)));
                        case 1:
                            return new ObservableCollection<ProductViewModel>(
                                _productService.Inventory.Where(i => i.FoundIn == ProductType.INVENTORY && (i.Name.ToUpper().Contains(Query.ToUpper())
                                    || i.Description.ToUpper().Contains(Query.ToUpper()))).OrderBy(i => i.Name)
                                .Select(i => new ProductViewModel(i)));
                        case 2:
                            return new ObservableCollection<ProductViewModel>(
                                _productService.Inventory.Where(i => i.FoundIn == ProductType.INVENTORY && (i.Name.ToUpper().Contains(Query.ToUpper())
                                    || i.Description.ToUpper().Contains(Query.ToUpper()))).OrderBy(i => i.Price)
                                .Select(i => new ProductViewModel(i)));
                    }
                }
            }
        }

        public ObservableCollection<CartViewModel> Carts
        {
            get
            {
                /*
                if (_productService == null)
                {
                    return new ObservableCollection<CartViewModel>();
                }
                */

                return new ObservableCollection<CartViewModel>(
                    _productService.CartList.Select(i => new CartViewModel(i)));
            }
        }

        public ObservableCollection<ProductViewModel> CartItems
        {
            get
            {
                if (SelectedCart == null)
                {
                    return new ObservableCollection<ProductViewModel>();
                }

                if (!string.IsNullOrEmpty(Query))
                {
                    switch(_sortMode)
                    {
                        default:
                        case 0:
                            return new ObservableCollection<ProductViewModel>(
                                _productService.Inventory.Where(i => (i.FoundIn == ProductType.CART && i.CartName.Equals(SelectedCart?.CartName))
                                && (i.Name.ToUpper().Contains(Query.ToUpper()) || i.Description.ToUpper().Contains(Query.ToUpper()))).OrderBy(i => i.Id)
                                .Select(i => new ProductViewModel(i)));
                        case 1:
                            return new ObservableCollection<ProductViewModel>(
                                _productService.Inventory.Where(i => (i.FoundIn == ProductType.CART && i.CartName.Equals(SelectedCart?.CartName))
                                && (i.Name.ToUpper().Contains(Query.ToUpper()) || i.Description.ToUpper().Contains(Query.ToUpper()))).OrderBy(i => i.Name)
                                .Select(i => new ProductViewModel(i)));
                        case 2:
                            return new ObservableCollection<ProductViewModel>(
                                _productService.Inventory.Where(i => (i.FoundIn == ProductType.CART && i.CartName.Equals(SelectedCart?.CartName))
                                && (i.Name.ToUpper().Contains(Query.ToUpper()) || i.Description.ToUpper().Contains(Query.ToUpper()))).OrderBy(i => i.TotalPrice)
                                .Select(i => new ProductViewModel(i)));
                    }
                }

                else
                {
                    switch(_sortMode)
                    {
                        default:
                        case 0:
                            return new ObservableCollection<ProductViewModel>(
                                _productService.Inventory.Where(i => i.FoundIn == ProductType.CART && i.CartName.Equals(SelectedCart?.CartName)).OrderBy(i => i.Id).Select(i => new ProductViewModel(i)));
                        case 1:
                            return new ObservableCollection<ProductViewModel>(
                                _productService.Inventory.Where(i => i.FoundIn == ProductType.CART && i.CartName.Equals(SelectedCart?.CartName)).OrderBy(i => i.Description).Select(i => new ProductViewModel(i)));
                        case 2:
                            return new ObservableCollection<ProductViewModel>(
                                _productService.Inventory.Where(i => i.FoundIn == ProductType.CART && i.CartName.Equals(SelectedCart?.CartName)).OrderBy(i => i.TotalPrice).Select(i => new ProductViewModel(i)));

                    }
                    return new ObservableCollection<ProductViewModel>(
                        _productService.Inventory.Where(i => i.FoundIn == ProductType.CART && i.CartName.Equals(SelectedCart?.CartName)).Select(i => new ProductViewModel(i)));
                }

            }
        }


        public async Task Add()
        {
            ContentDialog diag = new ProductDialog();
            await diag.ShowAsync();
            NotifyPropertyChanged("Products");
        }

        public async void Update()
        {
            //ContentDialog diag = new ProductDialog(p);
            //await diag.ShowAsync();
            //NotifyPropertyChanged("Products");

            if (SelectedProduct != null)
            {
                ContentDialog diag = new ProductDialog(SelectedProduct);
                if (SelectedProduct.IsProductByQuantity)
                {
                    diag = new QuantityDialog(SelectedProduct.BoundProductByQuantity);
                }
                else if (SelectedProduct.IsProductByWeight)
                {
                    diag = new WeightDialog(SelectedProduct.BoundProductByWeight);
                }

                await diag.ShowAsync();
                NotifyPropertyChanged("Products");
            }

        }

        public void Remove()
        {
            var id = SelectedProduct?.Id ?? -1;
            if (id >= 1)
            {
                _productService.Delete(SelectedProduct.Id);
            }
            NotifyPropertyChanged("Products");
        }

        public void Save()
        {
            _productService.Save();
        }

        public void Load()
        {
            _productService.Load();
            NotifyPropertyChanged("Products");
            NotifyPropertyChanged("Carts");
            NotifyPropertyChanged("CartItems");
        }

        public void Refresh()
        {
            NotifyPropertyChanged("Products");
            //NotifyPropertyChanged("Carts");
            NotifyPropertyChanged("CartItems");
        }

        //public List<string> cartList;

        public async void CreateCart()
        {
            ContentDialog diag = new CartDialog();
            await diag.ShowAsync();
            NotifyPropertyChanged("Carts");
        }

        public async void DeleteCart()
        {
            var name = SelectedCart?.CartName ?? string.Empty;
            if (!name.Equals(string.Empty))
                _productService.CartList.Remove(name);
            NotifyPropertyChanged("Carts");
        }

        public async void AddToCart()
        {
            ContentDialog diag;
            Product cartItem;

            //Product cartItem;
            var cartName = SelectedCart?.CartName ?? null;

            if (SelectedProduct.IsProductByQuantity)
            {
                cartItem = new ProductByQuantity
                {
                    Id = SelectedProduct.Id,
                    Name = SelectedProduct.Name,
                    Description = SelectedProduct.Description,
                    Price = SelectedProduct.BoundProduct.Price,
                    Quantity = SelectedProduct.Quantity,
                    FoundIn = ProductType.CART
                };

                diag = new AddQuantityToCartDialog((cartItem as ProductByQuantity), cartName);
                //diag = new AddQuantityToCartDialog((Select, cartName);
            }
            else
            {
                cartItem = new ProductByWeight()
                {
                    Id = SelectedProduct.Id,
                    Name = SelectedProduct.Name,
                    Description = SelectedProduct.Description,
                    Price = SelectedProduct.BoundProduct.Price,
                    Weight = SelectedProduct.BoundProductByWeight.Weight,
                    FoundIn = ProductType.CART
                };

                diag = new AddWeightToCartDialog((cartItem as ProductByWeight), cartName);
            }

            if (cartName != null)
            {
                await diag.ShowAsync();
            }
            NotifyPropertyChanged("CartItems");
            NotifyPropertyChanged("Products");
        }

        public async void EditCart()
        {
            if (SelectedCartItem != null && SelectedCart != null)
            {
                ContentDialog diag = new ProductDialog(SelectedProduct);
                if (SelectedCartItem.IsProductByQuantity)
                {
                    diag = new AddQuantityToCartDialog(SelectedCartItem.BoundProductByQuantity, SelectedCart.CartName);
                }
                else if (SelectedCartItem.IsProductByWeight)
                {
                    diag = new AddWeightToCartDialog(SelectedCartItem.BoundProductByWeight, SelectedCart.CartName);
                }

                await diag.ShowAsync();
                NotifyPropertyChanged("CartItems");
                NotifyPropertyChanged("Products");
            }
        }

        public void RemoveFromCart()
        {
            var id = SelectedCartItem?.Id ?? -1;
            if (id >= 1)
            {
                _productService.Delete(SelectedCartItem.Id, ProductType.CART, SelectedCartItem.BoundProduct.CartName);
            }
            NotifyPropertyChanged("Products");
            NotifyPropertyChanged("CartItems");
        }

        public void Sort()
        {
            _sortMode = (++_sortMode) % 3;
            NotifyPropertyChanged("Products");
            NotifyPropertyChanged("CartItems");
        }
    }
}
