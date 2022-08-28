namespace WhatToDo.Model;

public class ToDoItem
{
    //public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    //public string[] Details { get; set; }
    //public string Location { get; set; }
    //public string StartPeriod { get; set; }
    //public string EndPeriod { get; set; }
    //public string Priority { get; set; }
    //public User[] UsersInvolved { get; set; }

    public ToDoItem() { }

    public ToDoItem(string name, string describ)
    {
        Name = name;
        Description = describ; 
    }

    public ToDoItem(ToDoItem item)
    {
        //Id = item.Id;
        Name = item.Name;
        Description = item.Description;
    }

    public void Clone(ToDoItem item)
    {
        this.Name = item.Name;
        this.Description = item.Description;
    }
}

