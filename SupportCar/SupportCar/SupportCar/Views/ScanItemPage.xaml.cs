using SupportCar.Models;
using SupportCar.Services;
using SupportCar.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SupportCar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanItemPage : ContentView
    {
        public IDataBase DataBase;

        public ScanItemPage()
        {
            InitializeComponent();
        }

        public ScanItemPage(Item item)
        {
            try
            {
                InitializeComponent();
                this.LabelDescription.Text = item.Description;
                this.LabelName.Text = item.Name;
                this.ImageItem.Source = item.Image;
                this.LabelPrice.Text = "$ " + item.Price.ToString();
                this.GetRelatedItems(item);

                RelatedItemsListView.ItemSelected += async (object sender, SelectedItemChangedEventArgs e) =>
                {
                    var selectedItem = (Item)e.SelectedItem;
                    ScanItemPage scanItemPage = new ScanItemPage(selectedItem);
                    var page = new ContentPage()
                    {
                        Content = scanItemPage
                    };
                    await Navigation.PushModalAsync(page);
                };
            }
            catch (Exception e)
            {

                throw e;
            }
            
            //BindingContext = viewModel = new ScanItemlViewModel();
        }

        private List<Item> allRelatedItems;
        private void GetRelatedItems(Item item)
        {
            this.DataBase = new DataBase(false);
            var conn = this.DataBase.GetConnectionAsync();

            var relatedItems = this.DataBase.GetRelatedItems(item, conn).Result;

            allRelatedItems = new List<Item>();

            foreach (var i in relatedItems)
                allRelatedItems.Add(i);
            
            if (allRelatedItems != null)
                RelatedItemsListView.ItemsSource = allRelatedItems;
        }
    }
}
