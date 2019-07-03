using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using SupportCar.Models;
using SupportCar.Views;
using SQLite;
using SupportCar.Services;

namespace SupportCar.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Recents> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        private SQLiteAsyncConnection _conn;

        public ItemsViewModel()
        {
            Title = "Recents";
            Items = new ObservableCollection<Recents>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            DataBase = new DataBase(false);
            this._conn = DataBase.GetConnectionAsync();
            
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = DataBase.GetRecentItems(this._conn);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}