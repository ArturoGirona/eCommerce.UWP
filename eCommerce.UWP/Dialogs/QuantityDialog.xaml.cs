﻿using eCommerce.UWP.ViewModels;
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
    public sealed partial class QuantityDialog : ContentDialog
    {
        public QuantityDialog()
        {
            this.InitializeComponent();
        }

        public QuantityDialog(Product p)
        {
            this.InitializeComponent();
            this.DataContext = p;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //step 1: coerce datacontext into view model
            var viewModel = DataContext as ProductByQuantity;

            //step 2: use a conversion constructor from view model -> todo

            //step 3: interact with the service using models;
            ProductService.Current.AddOrUpdate(DataContext as ProductByQuantity);

        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void Set_Bogo_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as ProductByQuantity;

            viewModel.BoGo = true;
        }

        private void Unset_Bogo_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as ProductByQuantity;

            viewModel.BoGo = false;
        }

    }
}
