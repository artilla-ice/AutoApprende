using SupportCar.Models;
using SupportCar.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SupportCar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public IDataBase DataBase;

        public class Menu
        {
            public string Name { get; set; }
            public string Icon { get; set; }
            public Page Page { get; set; }
        }

        public List<Menu> MenuItems;

        public HomePage()
        {
            InitializeComponent();
            GetMenuItems();
            MenuListView.ItemSelected += async (object sender, SelectedItemChangedEventArgs e) =>
            {
                var item = (Menu)e.SelectedItem;
                ListMenuItemsPage list = new ListMenuItemsPage(item.Name);
                await Navigation.PushModalAsync(list);
            };

        }

        private async Task GetMenuItems()
        {
            List<Menu> menu = new List<Menu>
            {
                new Menu (){Name="Servicios", Icon="services_icon.png", Page= new ScanPage()},
                new Menu (){Name="Sistemas", Icon="car_icon.png", Page= new ScanPage()},
                new Menu (){Name="Piezas", Icon="pistons.png", Page= new ScanPage()}
            };

            MenuItems = new List<Menu>();

            foreach (var item in menu)
            {
                MenuItems.Add(item);
            }

            if (MenuItems !=null)
                 MenuListView.ItemsSource = MenuItems;
        }
    }
}