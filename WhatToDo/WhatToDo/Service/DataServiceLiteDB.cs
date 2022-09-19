

namespace WhatToDo.Service;

public class DataServiceLiteDB : IDataService
{

    public DataServiceLiteDB()
    {

    }

    #region File Path



    #endregion File Path


    #region Todo Items

    #region Get Todo Items

    public async Task<Dictionary<int, ToDoItem>> GetItemsAsync()
    {
        throw new NotImplementedException();
    }

    #endregion Get Todo Items

    #region Save Todo Items

    #endregion Save Todo Items

    #region Update Todo Items

    public async Task UpdateItemAsync(ToDoItem itemToUpdate)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateItemAsync(List<ToDoItem> itemsToUpdate)
    {
        throw new NotImplementedException();
    }

    #endregion Update Todo Items

    #region Delete Todo Items

    public async Task DeleteItemAsync(ToDoItem itemToDelete)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteItemAsync(List<ToDoItem> itemsToDelete)
    {
        throw new NotImplementedException();
    }

    #endregion Delete Todo Items

    #endregion Todo Items







    public async Task<GeolocCurrent> GetLastPositionAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ISessionService> GetSessionAsync(ISessionService session)
    {
        throw new NotImplementedException();
    }

    public async Task<Dictionary<DateTime, WeatherData>> GetWeatherAsync()
    {
        throw new NotImplementedException();
    }

    public async Task SaveLastPositionData(GeolocCurrent position)
    {
        throw new NotImplementedException();
    }

    public async Task SaveSession(ISessionService session)
    {
        throw new NotImplementedException();
    }

    public async Task SaveWeatherData(List<WeatherData> forecast)
    {
        throw new NotImplementedException();
    }


    #region Todo Items

    #region Get Todo Items

    #endregion Get Todo Items

    #region Save Todo Items

    #endregion Save Todo Items

    #region Update Todo Items

    #endregion Update Todo Items

    #region Delete Todo Items

    #endregion Delete Todo Items

    #endregion Todo Items
}
