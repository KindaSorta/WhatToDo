

using WhatToDo.Data;

namespace WhatToDo.ViewModel;

public partial class ToDoItemDetailsViewModel : BaseViewModel
{
    [ObservableProperty]
    ToDoItem itemText;

    [ObservableProperty]
    bool isPopUp;

    [ObservableProperty]
    bool wantsWeather;

    public bool IsNotPopUp => !IsPopUp;

    public List<string> Options { get; } = FormOptions.WeatherOptions;

    [ObservableProperty]
    float blur;

    IDataService dataService;
    ISessionService session;
    IDialogService dialogService;


    public ToDoItemDetailsViewModel(IDataService dataService, ISessionService session, IDialogService dialogService)
    {
        this.dataService = dataService;
        this.dialogService = dialogService;
        this.session = session;
        /*        Title = String.IsNullOrWhiteSpace(session.ItemToEdit.Name) ? "Add" : session.ItemToEdit.Name;
                Blur = 1;
                ItemText = new ToDoItem(session.ItemToEdit);
                WantsWeather = ItemText.HasWeatherPreference;
                if (ItemText.StartDate is null) ItemText.StartDate = DateTime.Now.Date;
                if (ItemText.DueDate is null) ItemText.DueDate = DateTime.Now.Date;*/
        Task.FromResult(InitialLoad());
    }

    async Task InitialLoad()
    {
        await Task.FromResult(Title = String.IsNullOrWhiteSpace(session.ItemToEdit.Name) ? "Add" : session.ItemToEdit.Name);
        await Task.FromResult(WantsWeather = session.ItemToEdit.HasWeatherPreference);
        await Task.FromResult(ItemText = new ToDoItem(session.ItemToEdit));
        await Task.Delay(100);
        await Task.FromResult(WantsWeather = session.ItemToEdit.HasWeatherPreference);
        if (!session.ItemToEdit.HasWeatherPreference) await Task.FromResult(ItemText.Weather = new WeatherPreference());
        if (ItemText.StartDate is null) await Task.FromResult(ItemText.StartDate = DateTime.Now.Date);
        if (ItemText.DueDate is null) await Task.FromResult(ItemText.DueDate = DateTime.Now.Date);
        await Task.FromResult(Blur = 1);
    }

    [RelayCommand]
    void TogglePopUp()
    {
        IsPopUp = !IsPopUp;
        Blur = IsPopUp ? 0.4f : 1f;
    }

    [RelayCommand]
    async Task SaveAsync()
    {
        if (IsBusy || !ItemText.IsValid()) return;

        IsBusy = true;

        if (ItemText.StartDate < DateTime.Now) ItemText.StartDate = null;
        if (ItemText.DueDate < DateTime.Now) ItemText.DueDate = null;
        ItemText.LastModifiedDate = DateTime.Now;
        session.ItemToEdit.CopyFrom(ItemText);

        await dataService.UpdateItemAsync(session.ItemToEdit);
        await Shell.Current.GoToAsync("..", true);

        IsBusy = false;
    }

    [RelayCommand]
    async Task DeleteAsync()
    {
        if (IsBusy || session.ItemToEdit.Id == 0) return;


        bool answer = await dialogService.DisplayAlert("Alert!!", "You are about to Delete a Task! Please confirm...", "OK", "Cancel");
        if (!answer) return;

        IsBusy = true;

        await dataService.DeleteItemAsync(session.ItemToEdit);
        session.ItemToEdit.Id = 0;

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
