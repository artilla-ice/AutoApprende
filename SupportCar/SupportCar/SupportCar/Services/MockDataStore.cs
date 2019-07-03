using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SupportCar.Models;

namespace SupportCar.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        List<Item> items;

        public MockDataStore()
        {
            items = new List<Item>();

            Random random = new Random();
            var mockItems = new List<Item>
            {
                new Item { Id = random.Next(1,99), Name = "First item", Description="This is an item description." },
                new Item { Id = random.Next(1,99), Name = "Second item", Description="This is an item description." },
                new Item { Id = random.Next(1,99), Name = "Third item", Description="This is an item description." },
                new Item { Id = random.Next(1,99), Name = "Fourth item", Description="This is an item description." },
                new Item { Id = random.Next(1,99), Name = "Fifth item", Description="This is an item description." },
                new Item { Id = random.Next(1,99), Name = "Sixth item", Description="This is an item description." },
            };

            foreach (var item in mockItems)
            {
                items.Add(item);
            }
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(int id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}