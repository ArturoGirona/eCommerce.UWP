﻿using eCommerce.UWP.ViewModels;
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
    public sealed partial class CartDialog : ContentDialog
    {
        public CartDialog()
        {
            this.InitializeComponent();
            this.DataContext = new CartViewModel();
        }

        public Visibility ErrorMessage { set; get; }
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //step 1: coerce datacontext into view model
            var viewModel = this.DataContext as CartViewModel;

            //step 2: use a conversion constructor from view model -> todo
            var cn = viewModel.CartName;
            //step 3: interact with the service using models;
            
            if (!ProductService.Current.CartList.Contains(cn))
                ProductService.Current.CartList.Add(cn);
            else
                ErrorMessage = Visibility.Visible;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
