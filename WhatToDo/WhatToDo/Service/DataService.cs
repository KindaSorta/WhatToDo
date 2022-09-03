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

        Dictionary<int, ToDoItem> Items { get; set; } = new Dictionary<int, ToDoItem>();

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
            if (itemToDelete == null || itemToDelete.Id == 0)
                return;
            await DeleteItemAsync(new List<ToDoItem>() { itemToDelete });
        }

        public async Task DeleteItemAsync(List<ToDoItem> itemsToDelete)
        {
            foreach (var item in itemsToDelete)
            {
                if (item != null && item.Id != 0)
                {
                    Items.Remove(item.Id);
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
                    if (Items.ContainsKey(item.Id))
                    {
                        Items[item.Id] = item;
                    }
                    else
                    {
                        int newId = Items.Count > 0 ? Items.Keys.Max() + 1 : 1;
                        Items[newId] = item;
                        Items[newId].Id = newId;
                    }
                    
                }
            }
            await dataStorage.WriteToFile(Items.Values.ToList(), "ToDoItems");
        }

        public async Task<Dictionary<int, ToDoItem>> GetItemsAsync()
        {
            List<ToDoItem> data = await dataStorage.ReadFromFile("ToDoItems");
            foreach (var item in data)
            {
                if (item != null && !String.IsNullOrEmpty(item.Name) && item.Id != 0)
                {
                    Items[item.Id] = item;
                }
            }
            return Items;
        }



    }
}
