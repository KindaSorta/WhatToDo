
namespace WhatToDo.ViewModel;

public class BaseViewModel : INotifyPropertyChanged
{
    bool isBusy;
    string title;

    public bool IsBusy { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    //CallerMemberName Attribute automatically gets the name of the property being called on
    public void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
