
namespace WhatToDo.ViewModel;

public partial class BaseViewModel : ObservableObject, IDisposable
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;

    [ObservableProperty]
    bool isRefreshing;

    [ObservableProperty]
    string title;

    public bool IsNotBusy => !IsBusy;
        

    public virtual void Dispose() 
    {
        title = default;
        isBusy = default;
        isRefreshing = default;
    }
}
