
namespace WhatToDo.Model;

public class Moment
{

    public DateTime? DateAndTime => IsScheduled ? DateOnly.FromDateTime(Date.Value).ToDateTime(TimeOnly.FromTimeSpan(Time.Value)) : null;

    public DateTime? Date { get; set; } = null; 
    public TimeSpan? Time { get; set; } = null;

    public bool IsScheduled => Date.HasValue && Time.HasValue;


    public Moment() { }

    public Moment(DateOnly date, TimeOnly time = new())
    {
        Date = date.ToDateTime(time);
        Time = time.ToTimeSpan();
    }

    public Moment(DateTime dateTime)
    {
        Date = dateTime;
        Time = TimeOnly.FromDateTime(dateTime).ToTimeSpan();
    }

    public void Clear()
    {
        Date = null;
        Time = null;
    }
}
