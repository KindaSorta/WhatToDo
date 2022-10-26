
//using static Android.Content.ClipData;

using WhatToDo.Data;

namespace WhatToDo.Model;


public class ToDoItem : IModelObject<ToDoItem>
{
    public ObjectId Id { get; set; } = ObjectId.Empty;
    public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public WeatherPreference Weather { get; set; } = null;
    public Moment StartDate { get; set; }
    public Moment DueDate { get; set; }
    public int Priority { get; set; } = 0;
    public bool IsComplete { get; set; } = false;
    public DateTime LastModifiedDate { get; set; }
    public List<DateTime> SuggestedDate { get; set; } = new List<DateTime>();

    public bool HasWeatherPreference => Weather != null;
    public Color Color => FormOptions.PriorityColors[Priority];
    public bool NotComplete => !IsComplete;
    public bool IsValidDueDate => DueDate.IsScheduled && DueDate.DateAndTime > DateTime.Now;
    public bool IsValidStartDate => StartDate.IsScheduled && DueDate.IsScheduled && StartDate.DateAndTime < DueDate.DateAndTime;
    //public User[] UsersInvolved { get; set; }
    //public string[] Details { get; set; }
    //public string Location { get; set; }

    public ToDoItem() { }

    public ToDoItem(string name, string describ, WeatherPreference weather, Moment start, Moment end, int priority, bool complete, DateTime lastMod, List<DateTime> suggestions)
    {
        Name = name;
        Description = describ;
        Weather = weather;
        StartDate = start;
        DueDate = end;
        Priority = priority;
        IsComplete = complete;
        LastModifiedDate = lastMod;
        SuggestedDate = suggestions;
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
        LastModifiedDate = item.LastModifiedDate;
        SuggestedDate = item.SuggestedDate;
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

    public ToDoItem(string name, Moment end, int priority, bool complete)
    {
        Name = name;
        DueDate = end;
        Priority = priority;
        IsComplete = complete;
    }

    public ToDoItem(DateTime now, WeatherPreference weather)
    {
        StartDate = new Moment(now);
        DueDate = new Moment(now);
        Weather = weather;
    }

    public void ToTemplate(ToDoItem item)
    {
        if (this.Id == item.Id) return;

        this.Name = item.Name;
        this.Description = item.Description;
        if (item.HasWeatherPreference) this.Weather = item.Weather;
        if (item.StartDate.IsScheduled) this.StartDate = item.StartDate;
        if (item.DueDate.IsScheduled) this.DueDate = item.DueDate;
        this.Priority = item.Priority;
        this.IsComplete = item.IsComplete;
        this.LastModifiedDate = item.LastModifiedDate;
        this.SuggestedDate = item.SuggestedDate;
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
        this.LastModifiedDate = item.LastModifiedDate;
        this.SuggestedDate = item.SuggestedDate;
    }
}

