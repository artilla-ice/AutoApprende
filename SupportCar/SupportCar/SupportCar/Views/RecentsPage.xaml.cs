using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SupportCar.Services;
using SQLite;
using SupportCar.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Android.Widget;

namespace SupportCar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecentsPage : ContentPage
    {
        public IDataBase DataBase;

        private ObservableCollection<Recents> m_RecentItems;

        public ObservableCollection<Recents> RecentItems
        {
            get { return m_RecentItems; }
            set { m_RecentItems = value; }
        }

        public RecentsPage()
        {
            try
            {

                InitializeComponent();
                this.DataBase = new DataBase(false);
                this.GetRecentItems();
                RecentsListView.ItemSelected += async (object sender, SelectedItemChangedEventArgs e) =>
                {
                    var item = (Item)e.SelectedItem;
                    ScanItemPage scanItemPage = new ScanItemPage(item)
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand
                    };
                    var page = new ContentPage()
                    {
                        Content = scanItemPage
                    };
                    await Navigation.PushModalAsync(page);
                };

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private List<Item> recentItems;
       

        public async void OnRefresh(object sender, EventArgs args)
        {
            var context = Android.App.Application.Context;
            var tostMessage = "Actualizando...";
            var duration = ToastLength.Long;

            var rndm = new Random();
            var delay = rndm.Next(500,3000);

            Toast.MakeText(context, tostMessage, duration).Show();
            System.Threading.Thread.Sleep(delay);
            await GetRecentItems();
        }

        private async Task GetRecentItems()
        {
            var _conn = new DataBase(false).GetConnectionAsync();

            this.RecentItems = new ObservableCollection<Recents>(this.DataBase.GetRecentItems(_conn));

            recentItems = new List<Item>();
            var allItems = this.DataBase.GetAllItems(_conn).Result;
            foreach (var item in RecentItems)
            {
                var newRecentItem = (from c in allItems
                                     where item.IdItem == c.Id
                                     select c).FirstOrDefault();

                recentItems.Add(newRecentItem);

            }
            if (recentItems != null)
                RecentsListView.ItemsSource = recentItems;
        }

       

    }
}
