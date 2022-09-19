using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToDo.Model
{
    public abstract class IndexedObject<T>
    {
        public int Id { get; set; }
        public T Value { get; set; }

        public IndexedObject(int id, T value)
        {
            Id = id;
            Value = value;
        }

        public virtual T BackToObject()
        {
            return this.Value;
        }
    }

    public class IndexedToDoItem : IndexedObject<ToDoItem>
    {
        public IndexedToDoItem(int id, ToDoItem item) : base(id, item) { }
    }
}
