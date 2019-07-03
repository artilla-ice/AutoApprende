using SQLite;
using SupportCar.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SupportCar.Services
{
    public interface IDataBase
    {
        SQLiteAsyncConnection GetConnectionAsync();
        Item GetItem(string name, SQLiteAsyncConnection conn);
        Item GetItemByID(int itemID, SQLiteAsyncConnection conn);
        List<Recents> GetRecentItems(SQLiteAsyncConnection conn);
        IEnumerable<string> GetItemData(int id, SQLiteAsyncConnection conn);
        Task<List<Item>> GetAllItems(SQLiteAsyncConnection conn);
        List<CarService> GetCarServices(SQLiteAsyncConnection conn);
        List<CarSystem> GetCarSystems(SQLiteAsyncConnection conn);

        Info GetInfo(string name, SQLiteAsyncConnection conn);
        Task<List<Item>> GetRelatedItems(Item mainItem, SQLiteAsyncConnection conn);

    }
}
