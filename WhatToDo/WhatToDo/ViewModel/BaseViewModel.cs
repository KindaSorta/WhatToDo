
namespace WhatToDo.ViewModel;

public partial class BaseViewModel : ObservableObject, IDisposable
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;

    [ObservableProperty]
    bool isRefreshing;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CacheCommand))]
    bool isCaching;

    [ObservableProperty]
    string title;

    public bool IsNotBusy => !IsBusy;

    [RelayCommand]
    public virtual async Task Cache() { return; }

    public virtual void Dispose() 
    {
        title = default;
        isBusy = default;
        isCaching = default;
        isRefreshing = default;
    }
}
