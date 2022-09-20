
//using static Android.Content.ClipData;

using WhatToDo.Data;

namespace WhatToDo.Model;


public class ToDoItem : IModelObject<ToDoItem>
{
    public ObjectId Id { get; set; } = ObjectId.Empty;
    public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public WeatherPreference Weather { get; set; } = null;
    public DateTime? StartDate { get; set; } = null;
    public DateTime? DueDate { get; set; } = null;
    public int Priority { get; set; } = 0;
    public bool IsComplete { get; set; } = false;
    public DateTime LastModifiedDate { get; set; }
    public List<DateTime> SuggestedDate { get; set; } = new List<DateTime>();

    public Color Color => FormOptions.PriorityColors[Priority];
    public bool NotComplete => !IsComplete;
    public bool HasDueDate => DueDate != null && DueDate > DateTime.Now;
    public bool HasStartDate => StartDate != null && StartDate > DateTime.Now;
    public bool HasWeatherPreference => Weather != null && !Weather.Equals(new WeatherPreference());
    //public User[] UsersInvolved { get; set; }
    //public string[] Details { get; set; }
    //public string Location { get; set; }

    public ToDoItem() { }

    public ToDoItem(string name, string describ, WeatherPreference weather, DateTime start, DateTime end, int priority, bool complete)
    {
        Name = name;
        Description = describ;
        Weather = weather;
        StartDate = start;
        DueDate = end;
        Priority = priority;
        IsComplete = complete;
    }

    public ToDoItem(ToDoItem item)
    {
        Id = item.Id;
        Name = item.Name;
        Description = item.Description;
        Weather = item.Weather;
        StartDate = item.StartDate;
        DueDate = item.DueDate;
        Priority = item.Priority;
        IsComplete = item.IsComplete;
    }

    public ToDoItem(string name, int priority, bool complete)
    {
        Name = name;
        Priority = priority;
        IsComplete = complete;
    }

    public ToDoItem(string name, string describ, int priority, bool complete)
    {
        Name = name;
        Description = describ;
        Priority = priority;
        IsComplete = complete;
    }

    public ToDoItem(string name, DateTime end, int priority, bool complete)
    {
        Name = name;
        DueDate = end;
        Priority = priority;
        IsComplete = complete;
    }

    public void DeepClone(ToDoItem item)
    {
        this.Id = item.Id;
        this.Name = item.Name;
        this.Description = item.Description;
        this.Weather = item.Weather;
        this.StartDate = item.StartDate;
        this.DueDate = item.DueDate;
        this.Priority = item.Priority;
        this.IsComplete = item.IsComplete;
    }

    public void CopyFrom(ToDoItem item)
    {
        this.Name = item.Name;
        this.Description = item.Description;
        this.Weather = item.Weather;
        this.StartDate = item.StartDate;
        this.DueDate = item.DueDate;
        this.Priority = item.Priority;
        this.IsComplete = item.IsComplete;
    }

    public void CopyTo(ToDoItem item)
    {
        item.Name = this.Name;
        item.Description = this.Description;
        item.Weather = this.Weather;
        item.StartDate = this.StartDate;
        item.DueDate = this.DueDate;
        item.Priority = this.Priority;
        item.IsComplete = this.IsComplete;

    }

    public bool IsValid()
    {
        if (String.IsNullOrWhiteSpace(Name) ||
            (HasDueDate && DueDate < DateTime.Now) || 
            (HasDueDate && HasStartDate && DueDate < StartDate))
        {
            return false;
        }

        return true;
    }
}

