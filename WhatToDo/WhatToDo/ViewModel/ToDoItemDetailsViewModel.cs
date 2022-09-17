

using WhatToDo.Data;

namespace WhatToDo.ViewModel;

[QueryProperty(nameof(CurrentItem), "CurrentItem")]
public partial class ToDoItemDetailsViewModel : BaseViewModel
{
    [ObservableProperty]
    ToDoItem currentItem;

    [ObservableProperty]
    ToDoItem itemText;

    [ObservableProperty]
    bool isPopUp;

    public bool IsNotPopUp => !IsPopUp;

    public List<string> Options { get; } = FormOptions.WeatherOptions;

    [ObservableProperty]
    float blur;

    IDataService dataService;
    ISessionService session;
    IDialogService dialogService;

    public ToDoItemDetailsViewModel(IDataService dataService, ISessionService session, IDialogService dialogService)
    {
        IsBusy = true;
        this.dataService = dataService;
        this.dialogService = dialogService;
        this.session = session;
        Blur = 1;
        Task.FromResult(InitializeAsync());
        IsBusy = false;
    }


    async Task InitializeAsync()
    {
        
        await Task.Delay(400);
        await Task.FromResult(Title = String.IsNullOrWhiteSpace(CurrentItem.Name) ? "Add" : CurrentItem.Name);
        ItemText = new ToDoItem(CurrentItem);
        if (ItemText.StartDate is null) await Task.FromResult(ItemText.StartDate = DateTime.Now.Date);
        if (ItemText.DueDate is null) await Task.FromResult(ItemText.DueDate = DateTime.Now.Date);
    }

    [RelayCommand]
    async Task TogglePopUpAsync()
    {
        await Task.FromResult(IsPopUp = !IsPopUp);
        await Task.FromResult(Blur = IsPopUp ? 0.4f : 1f);
    }

    [RelayCommand]
    async Task SaveAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        if (!ItemText.IsValid())
            return;
        else
        {
            if (ItemText.StartDate < DateTime.Now) await Task.FromResult(ItemText.StartDate = null);
            if (ItemText.DueDate < DateTime.Now) await Task.FromResult(ItemText.DueDate = null);
            if (!ItemText.Weather.Equals(new WeatherPreference()))
            {
                await dialogService.DisplayAlert("Invalid Input!", "", "OK");
            }
            CurrentItem.CopyFrom(ItemText);

            await dataService.UpdateItemAsync(CurrentItem);
            await Task.Delay(100);
            await Shell.Current.GoToAsync("..", true);
        }
        IsBusy = false;
    }

    [RelayCommand]
    async Task DeleteAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        if (CurrentItem.Id != 0)
        {
            await dataService.DeleteItemAsync(CurrentItem);
            CurrentItem.Id = 0;
        }
        await Task.Delay(100);
        await Shell.Current.GoToAsync("..", true);
        IsBusy = false;
    }

    public override void Dispose()
    {
        base.Dispose();
    }

    /*    [RelayCommand]
        async Task GoBackAsync()
        {

        }*/
}
