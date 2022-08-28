
using WhatToDo.Service;
using WhatToDo.Data;
using WhatToDo.View;

namespace WhatToDo.ViewModel;

[QueryProperty(nameof(CurrentItem), "item")]
public partial class ToDoItemDetailsViewModel : BaseViewModel
{
    [ObservableProperty]
    ToDoItem currentItem;

    [ObservableProperty]
    ToDoItem itemText;

    IDataService dataService;

    public ToDoItemDetailsViewModel(IDataService dataService)
    {
        this.dataService = dataService;
        Task.Run(async () =>
        {
            await Task.Delay(100);
            Title = CurrentItem == null ? "Add" : CurrentItem.Name;
            ItemText = new ToDoItem(CurrentItem);
        });
    }

    [RelayCommand]
    async Task SaveAsync()
    {
        if (String.IsNullOrWhiteSpace(ItemText.Name))
            return;
        else
        {
            CurrentItem.Clone(ItemText);
            await dataService.UpdateItemAsync(CurrentItem);
            await Task.Delay(100);
            await Shell.Current.GoToAsync("..", true);
        }
    }

/*    [RelayCommand]
    async Task GoBackAsync()
    {
        
    }*/
}
