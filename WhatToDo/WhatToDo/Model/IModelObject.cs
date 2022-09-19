

namespace WhatToDo.Model;

public interface IModelObject<T>
{
    void CopyFrom(T item);
    void CopyTo(T item);
    bool IsValid();
}
