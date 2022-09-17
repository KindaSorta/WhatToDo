
using System.Reflection;

namespace WhatToDo.Utilities;

public static class HelperFunctions
{
    public static bool Compare<T>(T a, T b)
    {
        return EqualityComparer<T>.Default.Equals(a, b);
    }

}
