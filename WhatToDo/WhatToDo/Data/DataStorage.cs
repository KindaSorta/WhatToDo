using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace WhatToDo.Data;

public class DataStorage
{
    Dictionary<string, ToDoItem> Items { get; set; } = new Dictionary<string, ToDoItem>();

    string path;

    public DataStorage()
    {
        path = Path.Combine(FileSystem.Current.AppDataDirectory, "WhatToDoApp");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        path = Path.Combine(path, "ToDoItems.txt");
        Task.Run(async() =>
        {
            Items = await GetItemsAsync();
        });
    }

    public async Task DeleteItemAsync(ToDoItem itemToDelete)
    {
        await Task.Run(() => {
            Items.Remove(itemToDelete.Name);
            var serializedData = JsonSerializer.Serialize(Items.Values.ToList());
            File.WriteAllText(path, serializedData);
        });
    }

    public async Task DeleteItemAsync(List<ToDoItem> itemsToDelete)
    {
        await Task.Run(() => {
            foreach (var item in itemsToDelete)
            {
                Items.Remove(item.Name);
            }
            var serializedData = JsonSerializer.Serialize(Items.Values.ToList());
            File.WriteAllText(path, serializedData);
        });
    }

    public async Task UpdateItemAsync(ToDoItem itemToUpdate)
    {
        await Task.Run(() => {
            Items[itemToUpdate.Name] = itemToUpdate;
            var serializedData = JsonSerializer.Serialize(Items.Values.ToList());
            File.WriteAllText(path, serializedData);
        });
    }

    public async Task UpdateItemAsync(List<ToDoItem> itemsToUpdate)
    {        
        await Task.Run(() => {
            foreach (var item in itemsToUpdate)
            {
                Items[item.Name] = item;
            }
            var serializedData = JsonSerializer.Serialize(Items.Values.ToList());
            File.WriteAllText(path, serializedData);
        });
    }

    public async Task<Dictionary<string, ToDoItem>> GetItemsAsync()
    {
        return await Task<Dictionary<string, ToDoItem>>.Run(() =>
        {
            var rawData = File.ReadAllText(path);
            if (!String.IsNullOrEmpty(rawData))
            {
                List<ToDoItem> data = JsonSerializer.Deserialize<List<ToDoItem>>(rawData);
                foreach (var item in data)
                {
                    Items[item.Name] = item;
                }
            }
            return Items;
        });
    }
}
