using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToDo.Utilities;

public static class ExtensionMethods
{
    public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items) =>
        items.ToList().ForEach(collection.Add);

}
