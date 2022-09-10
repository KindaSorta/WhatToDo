

namespace WhatToDo.Service;

public class DialogService : IDialogService
{
    public Task DisplayAlert(string title, string message, string accept, string cancel = null)
    {
        return Shell.Current.DisplayAlert(title, message, accept);
    }
}
