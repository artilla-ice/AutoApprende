using SupportCar.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SupportCar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu: MasterDetailPage
    {
        public Menu()
        {
            InitializeComponent();
            SetMenu();
        }

        public void SetMenu()
        {
            Detail = new NavigationPage(new ScanPage());

            List<MenuOrders> menu = new List<MenuOrders>
            {
                new MenuOrders(){Name="Piezas", Icon="settings.png", Page= new ScanPage()},
            };
            ListMenu.ItemsSource = menu;
        }

        private void ListMenu_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var menu = e.SelectedItem as MenuOrders;
            if (menu != null)
            {
                IsPresented = false;
                Detail = new NavigationPage(menu.Page);
            }
        }
        public class MenuOrders
        {
            public string Name { get; set; }
            public ImageSource Icon { get; set; }
            public Page Page { get; set; }
        }
    }
}