using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToDo.Data;

namespace WhatToDo.Service
{
    public class DataService : IDataService
    {
        DataStorage dataStorage;

        Dictionary<string, ToDoItem> Items { get; set; } = new Dictionary<string, ToDoItem>();

        public DataService()
        {
            dataStorage = new DataStorage();
            Task.Run(async () =>
            {
                Items = await GetItemsAsync();
            });
        }

        public async Task DeleteItemAsync(ToDoItem itemToDelete)
        {
            if (itemToDelete == null || String.IsNullOrEmpty(itemToDelete.Name))
                return;
            await DeleteItemAsync(new List<ToDoItem>() { itemToDelete });
        }

        public async Task DeleteItemAsync(List<ToDoItem> itemsToDelete)
        {
            foreach (var item in itemsToDelete)
            {
                if (item != null && !String.IsNullOrEmpty(item.Name))
                {
                    Items.Remove(item.Name);
                }
            }
            await dataStorage.WriteToFile(Items.Values.ToList(), "ToDoItems");
        }

        public async Task UpdateItemAsync(ToDoItem itemToUpdate)
        {
            if (itemToUpdate == null || String.IsNullOrEmpty(itemToUpdate.Name))
                return;            
            await UpdateItemAsync(new List<ToDoItem>() { itemToUpdate });
        }

        public async Task UpdateItemAsync(List<ToDoItem> itemsToUpdate)
        {
            foreach (var item in itemsToUpdate)
            {
                if (item != null && !String.IsNullOrEmpty(item.Name))
                {
                    Items[item.Name] = item;
                }
            }
            await dataStorage.WriteToFile(Items.Values.ToList(), "ToDoItems");
        }

        public async Task<Dictionary<string, ToDoItem>> GetItemsAsync()
        {
            List<ToDoItem> data = await dataStorage.ReadFromFile("ToDoItems");
            foreach (var item in data)
            {
                if (item != null && !String.IsNullOrEmpty(item.Name))
                {
                    Items[item.Name] = item;
                }
            }
            return Items;
        }



    }
}
