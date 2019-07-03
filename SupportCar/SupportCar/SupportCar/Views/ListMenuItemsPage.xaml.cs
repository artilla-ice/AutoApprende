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
    public partial class ListMenuItemsPage : ContentPage
    {
        public IDataBase DataBase;

        public class Menu
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Image { get; set; }
            public int Price { get; set; }
            public int SharedNumber { get; set; }
            public string FolderName { get; set; }

        }

        private ObservableCollection<CarService> m_carServices;

        public ObservableCollection<CarService> CarServices
        {
            get { return m_carServices; }
            set { m_carServices = value; }
        }

        private ObservableCollection<CarSystem> m_carSystems;

        public ObservableCollection<CarSystem> CarSystems
        {
            get { return m_carSystems; }
            set { m_carSystems = value; }
        }

        private ObservableCollection<Item> m_items;

        public ObservableCollection<Item> ListItems
        {
            get { return m_items; }
            set { m_items = value; }
        }



        public ListMenuItemsPage()
        {
            InitializeComponent();
            this.DataBase = new DataBase(false);
        }

        public string TypeItem;
        public ListMenuItemsPage(string typeItem)
        {
            InitializeComponent();
            this.DataBase = new DataBase(false);
            this.TypeItem = typeItem;
            TypeItemLabel.Text = TypeItem;
            GetItems();
            MenuListView.ItemSelected += async (object sender, SelectedItemChangedEventArgs e) =>
            {
                View page = null;
                var model = (Menu)e.SelectedItem;
                switch (TypeItem)
                {
                    case "Servicios":
                        var service = new CarService()
                        {
                            Id = model.Id,
                            Name = model.Name,
                            FolderName = model.FolderName

                        };
                        page = new ServicesPage(service)
                        {
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.FillAndExpand
                        };
                        break;
                    case "Sistemas":
                        var system = new CarSystem()
                        {
                            Id = model.Id,
                            Name = model.Name,
                            FolderName = model.FolderName

                        };
                        page = new ServicesPage(null, system)
                        {
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.FillAndExpand
                        };
                        break;
                    case "Piezas":
                        try
                        {


                            var item = new Item()
                            {
                                Id = model.Id,
                                Name = model.Name,
                                Description = model.Description,
                                Price = model.Price,
                                Image = model.Image,
                                SharedNumber = model.SharedNumber
                            };

                            page = new ScanItemPage(item)
                            {
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                VerticalOptions = LayoutOptions.FillAndExpand
                            };
                            break;
                        }
                        catch (Exception ex)
                        {

                            throw ex;
                        }

                    default:
                        break;
                }
                var contentPage = new ContentPage()
                {
                    Content = page
                };
                if (contentPage != null)
                    await Navigation.PushModalAsync(contentPage);
            };

            
}


        private List<Menu> Items;
        private async Task GetItems()
        {
            var _conn = new DataBase(false).GetConnectionAsync();
            Items = new List<Menu>();
            if(TypeItem != null)
            {
                switch (TypeItem)
                {
                    case "Servicios":
                    {
                            try
                            {
                                CarServices = new ObservableCollection<CarService>(this.DataBase.GetCarServices(_conn));

                                foreach (var item in CarServices)
                                    Items.Add(new Menu()
                                    {
                                        Id = item.Id,
                                        FolderName = item.FolderName,
                                        Name = item.Name
                                        
                                    });
                                

                                if (Items != null)
                                    MenuListView.ItemsSource = Items;

                                break;
                            }
                            catch (Exception ex)
                            {

                                throw ex;
                            }

                           
                    }
                    case "Piezas":
                        {
                            ListItems = new ObservableCollection<Item>(this.DataBase.GetAllItems(_conn).Result);

                            foreach (var item in ListItems)
                            {
                                var newItem = new Menu() { Id=item.Id,Name = item.Name, Description= item.Description, Image=item.Image, Price = item.Price, SharedNumber=item.SharedNumber};
                                Items.Add(newItem);
                            }

                            if (Items != null)
                                MenuListView.ItemsSource = Items;

                            break;
                        }
                    case "Sistemas":
                        {
                            try
                            {
                                CarSystems = new ObservableCollection<CarSystem>(this.DataBase.GetCarSystems(_conn));

                                foreach (var item in CarSystems)
                                    Items.Add(new Menu()
                                    {
                                        Id = item.Id,
                                        FolderName = item.FolderName,
                                        Name = item.Name

                                    });


                                if (Items != null)
                                    MenuListView.ItemsSource = Items;

                                break;
                            }
                            catch (Exception ex)
                            {

                                throw ex;
                            }


                        }
                    default:
                        break;
                }
            }
            
        }

        enum Types
        {
            Servicios,
            Sistemas,
            Piezas
        }
    }
}