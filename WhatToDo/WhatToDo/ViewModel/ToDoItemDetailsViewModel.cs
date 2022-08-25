
using WhatToDo.Service;
using WhatToDo.Data;
using WhatToDo.View;

namespace WhatToDo.ViewModel;

[QueryProperty(nameof(CurrentItem), "item")]
public partial class ToDoItemDetailsViewModel : BaseViewModel
{
    [ObservableProperty]
    ToDoItem currentItem;

    DataStorage dataStorage;

    public ToDoItemDetailsViewModel(DataStorage dataStorage)
    {
        Title = CurrentItem == null ? "Add" : CurrentItem.Name;
        this.dataStorage = dataStorage;
    }

    [RelayCommand]
    async Task SaveAsync()
    {
        if (String.IsNullOrWhiteSpace(CurrentItem.Name))
            return;
        else
        {
            await dataStorage.UpdateItemAsync(CurrentItem);
            await Shell.Current.GoToAsync("..");
        }
    }

/*        [RelayCommand]
    void GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }*/
}
