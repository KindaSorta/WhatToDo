

namespace WhatToDo.Service;

public class DialogService : IDialogService
{
    public Task DisplayAlert(string title, string message, string accept)
    {
        return Shell.Current.DisplayAlert(title, message, accept);
    }

    public Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
    {
        return Shell.Current.DisplayAlert(title, message, accept, cancel);
    }
}
