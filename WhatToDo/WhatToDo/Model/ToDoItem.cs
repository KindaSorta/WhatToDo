
namespace WhatToDo.Model;


public class ToDoItem
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public WeatherType Weather { get; set; } = null;
    public DateTime? StartDate { get; set; } = null;
    public DateTime? DueDate { get; set; } = null;
    public float Priority { get; set; } = 0f;
    public bool IsComplete { get; set; } = false;
    public bool HasDueDate => DueDate != null && DueDate > DateTime.Now ? true : false;
    public bool HasStartDate => StartDate != null && StartDate > DateTime.Now ? true : false;
    //public User[] UsersInvolved { get; set; }
    //public string[] Details { get; set; }
    //public string Location { get; set; }

    public ToDoItem() { }

    public ToDoItem(string name, string describ, WeatherType weather, DateTime start, DateTime end, float priority, bool complete)
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

    public ToDoItem(string name, float priority, bool complete)
    {
        Name = name;
        Priority = priority;
        IsComplete = complete;
    }

    public ToDoItem(string name, string describ, float priority, bool complete)
    {
        Name = name;
        Description = describ;
        Priority = priority;
        IsComplete = complete;
    }

    public ToDoItem(string name, DateTime end, float priority, bool complete)
    {
        Name = name;
        DueDate = end;
        Priority = priority;
        IsComplete = complete;
    }

    public void Clone(ToDoItem item)
    {
        this.Name = item.Name;
        this.Description = item.Description;
        this.Weather = item.Weather;
        this.StartDate = item.StartDate;
        this.DueDate = item.DueDate;
        this.Priority = item.Priority;
        this.IsComplete = item.IsComplete;
    }
}

