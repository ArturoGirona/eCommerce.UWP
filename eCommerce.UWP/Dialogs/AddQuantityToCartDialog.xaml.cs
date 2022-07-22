using eCommerce.UWP.ViewModels;
using Library.eCommerce.Models;
using Library.eCommerce.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace eCommerce.UWP.Dialogs
{
    public sealed partial class AddQuantityToCartDialog : ContentDialog
    {

        private ProductByQuantity _product;
        private string _cartName;
        public AddQuantityToCartDialog()
        {
            this.InitializeComponent();
            //this.DataContext = new AddToCartViewModel();
            this.DataContext = new ProductViewModel();
        }

        public AddQuantityToCartDialog(ProductByQuantity p, string cn)
        {
            this.InitializeComponent();
            //this.DataContext = new AddToCartViewModel();
            this.DataContext = new ProductViewModel();

            this._product = p;
            this._cartName = cn;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var viewModel = this.DataContext as ProductViewModel;

            var cartItem = new ProductByQuantity();

            cartItem.Id = _product.Id;
            cartItem.Name = _product.Name;
            cartItem.Description = _product.Description;
            cartItem.Price = (decimal)_product.Price;
            cartItem.Quantity = viewModel.Quantity;
            cartItem.FoundIn = ProductType.CART;
            cartItem.CartName = _cartName;
            
            ProductService.Current.AddOrUpdate(cartItem, ProductType.CART, _cartName);

        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
