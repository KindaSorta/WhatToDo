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
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WhatToDo.Data;

public class FileStorage
{
    string DirectoryPath;

    public FileStorage()
    {
        DirectoryPath = Path.Combine(FileSystem.Current.AppDataDirectory, "WhatToDoApp");
        if (!Directory.Exists(DirectoryPath))
        {
            Directory.CreateDirectory(DirectoryPath);
        }
    }

    public async Task WriteToFile<T>(List<T> items, string fileName)
    {
        string filePath = Path.Combine(DirectoryPath, $"{fileName}.txt");
        await Task.Run(() => {
            var serializedData = JsonSerializer.Serialize(items);
            File.WriteAllText(filePath, serializedData);
        });
    }

    public async Task<List<T>> ReadFromFile<T>(string fileName)
    {
        return await Task<List<T>>.Run(() =>
        {
            if (Directory.GetFiles(DirectoryPath, $"{fileName}.txt").Length > 0)
            {
                string filePath = Path.Combine(DirectoryPath, $"{fileName}.txt");
                var rawData = File.ReadAllText(filePath);
                if (!String.IsNullOrEmpty(rawData))
                {
                    return JsonSerializer.Deserialize<List<T>>(rawData).ToList();
                }
            }
            return new List<T>();
        });
    }
}
