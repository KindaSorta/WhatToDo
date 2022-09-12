using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WhatToDo.Model;

namespace WhatToDo.Service;

public class WeatherService
{
    List<WeatherData> data;
    HttpClient httpClient;


    public WeatherService()
    {
        data = new List<WeatherData>();
        this.httpClient = new HttpClient();
    }

    public async Task<List<WeatherData>> GetWeather(Location location)
    {
        string forcastURL = String.Empty;

        string coordinatesURL = "https://api.weather.gov/points/" + location.Latitude.ToString("0.####") + "," + location.Longitude.ToString("0.####");

        //Request for Weather API coordinates based off current Geo location
        var productValue = new ProductInfoHeaderValue("WhatToDoApp", "1.0");
        var commentValue = new ProductInfoHeaderValue("(Software Dev Student)");
        //var request = new HttpRequestMessage(HttpMethod.Get, "https://api.weather.gov/points/32.3617,-86.2792");
        var request = new HttpRequestMessage(HttpMethod.Get, coordinatesURL);

        request.Headers.UserAgent.Add(productValue);
        request.Headers.UserAgent.Add(commentValue);
        var response = await httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            //Get the location specific Url from Weather API for the forecast periods
            var result = await response.Content.ReadAsStringAsync();
            JObject jsonTree = (JObject)JsonConvert.DeserializeObject(result);
            forcastURL = jsonTree["properties"]["forecast"].ToString();
        }

        if (forcastURL != null) 
        {
            request = new HttpRequestMessage(HttpMethod.Get, forcastURL);
            request.Headers.UserAgent.Add(productValue);
            request.Headers.UserAgent.Add(commentValue);
            response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                JObject jsonTree = (JObject)JsonConvert.DeserializeObject(result);
                var periods = jsonTree["properties"]["periods"].ToList();
                foreach (var item in periods)
                {
                    data.Add(JsonConvert.DeserializeObject<WeatherData>(item.ToString()));
                }
            }
        }
        return data;

    }
}
