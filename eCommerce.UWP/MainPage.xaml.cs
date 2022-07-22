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
using Library.eCommerce.Services;
using Library.eCommerce.Models;
using eCommerce.UWP.ViewModels;
using static Library.eCommerce.Services.ProductService;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using Windows.UI.Core.Preview;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace eCommerce.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = new eCommerceViewModel();
            ProductService.Current.Load();

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void Add_New_Product_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as eCommerceViewModel;
            if (vm != null)
            {
                await vm.Add();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as eCommerceViewModel).Save();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as eCommerceViewModel).Load();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            //var model = ((sender as Button).DataContext as ProductViewModel);
            //await (DataContext as eCommerceViewModel).Update(model);

            var vm = DataContext as eCommerceViewModel;
            if (vm != null)
            {
                vm.Update();
            }

        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as eCommerceViewModel;
            if (vm != null)
            {
                vm.Remove();
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as eCommerceViewModel).Refresh();
        }

        private void Add_To_Cart_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as eCommerceViewModel;

            vm.AddToCart();
        }

        private void Edit_Cart_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as eCommerceViewModel;

            vm.EditCart();
        }

        private void Remove_From_Cart_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as eCommerceViewModel;

            vm.RemoveFromCart();
        }

        private void Create_Cart_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as eCommerceViewModel;
            vm.CreateCart();
        }

        private void Delete_Cart_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as eCommerceViewModel;
            vm.DeleteCart();
        }

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as eCommerceViewModel;
            vm.Sort();

        }
    }
}
