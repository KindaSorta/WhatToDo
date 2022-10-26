

namespace WhatToDo.Model;

public interface IModelObject<T>
{
    void CopyFrom(T item);
}
