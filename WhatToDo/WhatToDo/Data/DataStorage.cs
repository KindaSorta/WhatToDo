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
    string DirectoryPath;

    public DataStorage()
    {
        DirectoryPath = Path.Combine(FileSystem.Current.AppDataDirectory, "WhatToDoApp");
        if (!Directory.Exists(DirectoryPath))
        {
            Directory.CreateDirectory(DirectoryPath);
        }
    }

    public async Task WriteToFile(List<ToDoItem> items, string fileName)
    {
        string filePath = Path.Combine(DirectoryPath, $"{fileName}.txt");
        await Task.Run(() => {
            var serializedData = JsonSerializer.Serialize(items);
            File.WriteAllText(filePath, serializedData);
        });
    }

    public async Task<List<ToDoItem>> ReadFromFile(string fileName)
    {
        return await Task<List<ToDoItem>>.Run(() =>
        {
            if (Directory.GetFiles(DirectoryPath, $"{fileName}.txt").Length > 0)
            {
                string filePath = Path.Combine(DirectoryPath, $"{fileName}.txt");
                var rawData = File.ReadAllText(filePath);
                if (!String.IsNullOrEmpty(rawData))
                {
                    return JsonSerializer.Deserialize<List<ToDoItem>>(rawData).ToList();
                }
            }
            return new List<ToDoItem>();
        });
    }
}
