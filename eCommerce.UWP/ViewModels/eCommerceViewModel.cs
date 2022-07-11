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
using static Library.eCommerce.Services.ProductService;
using eCommerce.UWP.Dialogs;
using Windows.UI.Core.Preview;

namespace eCommerce.UWP.ViewModels
{
    public class eCommerceViewModel : INotifyPropertyChanged
    {
        public string Query { get; set; }
        public ProductViewModel SelectedProduct { get; set; }
        public CartViewModel SelectedCart { get; set; }
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
                    return new ObservableCollection<ProductViewModel>(_productService.Inventory.Select(i => new ProductViewModel(i)));
                }
                else
                {
                    return new ObservableCollection<ProductViewModel>(
                        _productService.Inventory.Where(i => i.Name.ToUpper().Contains(Query.ToUpper())
                            || i.Description.ToUpper().Contains(Query.ToUpper()))
                        .Select(i => new ProductViewModel(i)));
                }
            }
        }

        public ObservableCollection<CartViewModel> Carts
        {
            get
            {
                if (_productService == null)
                {
                    return new ObservableCollection<CartViewModel>();
                }

                return new ObservableCollection<CartViewModel>(
                    _productService.CartList.Select(i => new CartViewModel(i)));
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
        }

        public void Refresh()
        {
            NotifyPropertyChanged("Products");
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
    }
}
