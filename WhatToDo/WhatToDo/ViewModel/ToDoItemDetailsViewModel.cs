

using WhatToDo.Data;
//using static Java.Util.Jar.Attributes;

namespace WhatToDo.ViewModel;

public partial class ToDoItemDetailsViewModel : BaseViewModel
{
    [ObservableProperty]
    ToDoItem itemText = new(DateTime.Now, new WeatherPreference());

    [ObservableProperty]
    bool isPopUp;

    public bool IsNotPopUp => !IsPopUp;

    public List<string> Options { get; } = FormOptions.WeatherOptions;

    [ObservableProperty]
    bool dateSelected;

    [ObservableProperty]
    bool isTimePeriod;

    [ObservableProperty]
    bool assignWeather;

    [ObservableProperty]
    float blur = 1;

    IDataService dataService;
    ISessionService session;
    IDialogService dialogService;


    public ToDoItemDetailsViewModel(IDataService dataService, ISessionService session, IDialogService dialogService)
    {
        this.dataService = dataService;
        this.dialogService = dialogService;
        this.session = session;

        Task.FromResult(InitialLoad());
    }

    async Task InitialLoad()
    {
        await Task.FromResult(Title = String.IsNullOrWhiteSpace(session.ItemToEdit.Name) ? "Add" : session.ItemToEdit.Name);
        await Task.FromResult(AssignWeather = session.ItemToEdit.HasWeatherPreference);
        await Task.FromResult(DateSelected = session.ItemToEdit.DueDate.IsScheduled);
        await Task.FromResult(IsTimePeriod = session.ItemToEdit.StartDate.IsScheduled);
        ItemText.ToTemplate(session.ItemToEdit);
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
        if (IsBusy || String.IsNullOrWhiteSpace(ItemText.Name)) return;
        IsBusy = true;

        if (DateSelected && (!ItemText.IsValidDueDate || 
            (IsTimePeriod && !ItemText.IsValidStartDate)))
        {
            await dialogService.DisplayAlert("Invalid Input!", "Please use valid date times.", "OK");
            IsBusy = false;
            return;
        }

        if (AssignWeather && !ItemText.Weather.IsValid())
        {
            await dialogService.DisplayAlert("Invalid Input!", "Please use valid weather data.", "OK");
            IsBusy = false;
            return;
        }

        if (!DateSelected) ItemText.DueDate.Clear();
        if (!DateSelected || !IsTimePeriod) ItemText.StartDate.Clear();

        if (!AssignWeather) ItemText.Weather = null;

        ItemText.LastModifiedDate = DateTime.Now;
        session.ItemToEdit.CopyFrom(ItemText);

        await dataService.SaveItem(session.ItemToEdit);
        await Shell.Current.GoToAsync("..", true);

        IsBusy = false;
    }

    [RelayCommand]
    async Task DeleteAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        if (session.ItemToEdit.Id != ObjectId.Empty)
        {
            bool answer = await dialogService.DisplayAlert("Alert!!", "You are about to Delete a Task! Please confirm...", "OK", "Cancel");
            if (!answer)
            {
                IsBusy = false;
                return;
            }

            await dataService.DeleteItem(session.ItemToEdit);
            session.ItemToEdit.Id = ObjectId.Empty;
        }

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
