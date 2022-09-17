using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToDo.Model
{
    public interface IModelObject<T>
    {
        void CopyFrom(T item);
        void CopyTo(T item);
        bool IsValid();
    }
}
