

using LiteDB;
//using static Android.Content.ClipData;

namespace WhatToDo.Service;

public class DataServiceLiteDB : IDataService
{

    public DataServiceLiteDB()
    {

    }

    #region File Path

    private async Task<string> GetConnectionString(string name)
    {
        var path = FileSystem.Current.AppDataDirectory;

        var fullPath = Path.Combine(path, $"{name}.db");

        // TODO: Setup Secure Storage and password
        var password = "password";

        /*var password = await secureStorageService.Get("password");

        if (password == null)
        {
            password = Guid.NewGuid().ToString();
            await secureStorageService.Save("password", password);
        }*/

        return await Task.FromResult($"Filename={fullPath};Password={password}");
    }


    #endregion File Path


    #region Todo Items

    #region Get Todo Items

    public async Task<List<ToDoItem>> GetAllItems()
    {
        using var db = new LiteDatabase(await GetConnectionString(nameof(ToDoItem)));

        var collection = db.GetCollection<ToDoItem>(nameof(ToDoItem));

        var items = collection.Query().ToList();

        return items ??= new();
    }

    #endregion Get Todo Items

    #region Save Todo Items

    public async Task SaveItem(ToDoItem item)
    {
        if (item == null) throw new ArgumentNullException(nameof(ToDoItem));

        if (String.IsNullOrWhiteSpace(item.Name)) throw new ArgumentException(nameof(ToDoItem));

        if (item.Id == ObjectId.Empty) item.Id = ObjectId.NewObjectId();

        using var db = new LiteDatabase(await GetConnectionString(nameof(ToDoItem)));

        var collection = db.GetCollection<ToDoItem>(nameof(ToDoItem));        

        if (collection.Query().Where(x => x.Id.Equals(item.Id)).ToList().Count > 0)
        {
            collection.Update(item);
        }
        else
        {
            collection.Insert(item);
            collection.EnsureIndex(x => x.Id);
        }
    }

    public async Task SaveManyItem(List<ToDoItem> items)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));

        if (items.Count <= 0) throw new ArgumentException(nameof(items));

        using var db = new LiteDatabase(await GetConnectionString(nameof(ToDoItem)));

        var collection = db.GetCollection<ToDoItem>(nameof(ToDoItem));

        foreach (var item in items)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            if (String.IsNullOrWhiteSpace(item.Name)) throw new ArgumentException(nameof(item));

            if (item.Id == ObjectId.Empty) item.Id = ObjectId.NewObjectId();

            if (collection.Query().Where(x => x.Id.Equals(item.Id)).ToList().Count > 0)
            {
                collection.Update(item);
            }
            else
            {
                collection.Insert(item);
                collection.EnsureIndex(x => x.Id);
            }
        }
    }

    #endregion Save Todo Items

    #region Delete Todo Items

    public async Task DeleteItem(ToDoItem item)
    {
        if (item == null) throw new ArgumentNullException(nameof(ToDoItem));

        if (item.Id == ObjectId.Empty) throw new ArgumentException(nameof(ToDoItem));

        using var db = new LiteDatabase(await GetConnectionString(nameof(ToDoItem)));

        var collection = db.GetCollection<ToDoItem>(nameof(ToDoItem));

        BsonValue id = item.Id;

        collection.Delete(id);
    }

    #endregion Delete Todo Items

    #endregion Todo Items


    #region Geolocation

    #region Get Geolocation

    public async Task<List<GeolocCurrent>> GetGeolocation()
    {
        using var db = new LiteDatabase(await GetConnectionString(nameof(GeolocCurrent)));

        var collection = db.GetCollection<GeolocCurrent>(nameof(GeolocCurrent));

        var geolocs = collection.Query().ToList();

        
        if (geolocs.Count > 1)
        {
            collection.DeleteAll();
            geolocs = geolocs.OrderBy(x => x.LastUpdate - DateTime.Now).ToList();
        }

        return geolocs ??= new();
    }

    #endregion Get Geolocation

    #region Save Geolocation

    public async Task SaveGeolocation(GeolocCurrent geoloc)
    {
        if (geoloc == null || geoloc.Location == null) throw new ArgumentNullException(nameof(GeolocCurrent));

       
        using var db = new LiteDatabase(await GetConnectionString(nameof(GeolocCurrent)));

        var collection = db.GetCollection<GeolocCurrent>(nameof(GeolocCurrent));

        if (collection.Query().Where(x => x.LastUpdate.Equals(geoloc.LastUpdate)).ToList().Count > 0)
        {
            collection.DeleteAll();
        }
        else
        {
            collection.Insert(geoloc);
            collection.EnsureIndex(x => x.LastUpdate);
        }
    }

    #endregion Save Geolocation

    #region Delete Geolocation

    public async Task DeleteAllGeolocation()
    {
        using var db = new LiteDatabase(await GetConnectionString(nameof(GeolocCurrent)));

        var collection = db.GetCollection<GeolocCurrent>(nameof(GeolocCurrent));

        collection.DeleteAll();
    }

    #endregion Delete Geolocation

    #endregion Geolocation


    #region WeatherData

    #region Get WeatherData

    public async Task<List<WeatherData>> GetAllWeatherData()
    {
        using var db = new LiteDatabase(await GetConnectionString(nameof(WeatherData)));

        var collection = db.GetCollection<WeatherData>(nameof(WeatherData));

        var data = collection.Query().ToList();

        return data ??= new();
    }

    #endregion Get WeatherData

    #region Save WeatherData

    public async Task SaveWeatherData(WeatherData data)
    {
        if (data == null) throw new ArgumentNullException(nameof(WeatherData));

        using var db = new LiteDatabase(await GetConnectionString(nameof(WeatherData)));

        var collection = db.GetCollection<WeatherData>(nameof(WeatherData));

        if (collection.Query().Where(x => x.startTime.Equals(data.startTime)).ToList().Count > 0)
        {
            collection.DeleteAll();
        }
        else
        {
            collection.Insert(data);
            collection.EnsureIndex(x => x.startTime);
        }
    }

    public async Task SaveManyWeatherData(List<WeatherData> items)
    {
        if (items == null) throw new ArgumentNullException(nameof(WeatherData));

        if (items.Count <= 0) throw new ArgumentException(nameof(WeatherData));

        using var db = new LiteDatabase(await GetConnectionString(nameof(WeatherData)));

        var collection = db.GetCollection<WeatherData>(nameof(WeatherData));

        foreach (var data in items)
        {
            if (data == null) throw new ArgumentNullException(nameof(WeatherData));

            if (collection.Query().Where(x => x.startTime.Equals(data.startTime)).ToList().Count > 0)
            {
                collection.DeleteAll();
            }
            else
            {
                collection.Insert(data);
                collection.EnsureIndex(x => x.startTime);
            }
        }
    }

    #endregion Save WeatherData

    #region Delete WeatherData

    public async Task DeleteAllWeatherData(WeatherData data)
    {
        if (data == null) throw new ArgumentNullException(nameof(WeatherData));

        using var db = new LiteDatabase(await GetConnectionString(nameof(WeatherData)));

        var collection = db.GetCollection<WeatherData>(nameof(WeatherData));

        collection.DeleteAll();
    }

    #endregion Delete WeatherData

    #endregion WeatherData



    #region Session

    #region Save Session

    public async Task SaveSession(SessionService session)
    {
        await SaveGeolocation(session.CurrentGeolocInfo);
        await SaveManyWeatherData(session.WeatherForcast);
    }

    #endregion Save Session

    #endregion Session

}
