
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
    int forecastSelection;

    [ObservableProperty]
    int highTempSelection;

    [ObservableProperty]
    int lowTempSelection;

    [ObservableProperty]
    int windSpeedSelection;

    [ObservableProperty]
    ToDoItem itemText;

    public List<string> ForecastOptions { get; set; } = ForecastDescrib.KeyWords;
    public List<int> TempRange { get; set; } = Enumerable.Range(-51, 130).ToList();
    public List<int> WindSpeeds { get; set; } = Enumerable.Range(0, 50).ToList();

    IDataService dataService;
    ISessionService session;

    public ToDoItemDetailsViewModel(IDataService dataService, ISessionService session)
    {
        this.dataService = dataService;
        this.session = session;
        Task.Run(async () =>
        {
            await Task.Delay(350);
            await Task.FromResult(Title = String.IsNullOrWhiteSpace(CurrentItem.Name) ? "Add" : CurrentItem.Name);
            await Task.FromResult(ItemText = new ToDoItem(CurrentItem));
            await FormateDisplayItem();
        });
    }

    public async Task FormateDisplayItem()
    {
        if (ItemText.StartDate is null) await Task.FromResult(ItemText.StartDate = DateTime.Now.Date);
        if (ItemText.DueDate is null) await Task.FromResult(ItemText.DueDate = DateTime.Now.Date);
        if (ItemText.Weather.High != "") await Task.FromResult(HighTempSelection = TempRange.IndexOf(Int32.Parse(ItemText.Weather.High)));
        if (ItemText.Weather.Low != "") await Task.FromResult(LowTempSelection = TempRange.IndexOf(Int32.Parse(ItemText.Weather.High)));
        if (ItemText.Weather.WindSpeed != "") await Task.FromResult(WindSpeedSelection = TempRange.IndexOf(Int32.Parse(ItemText.Weather.High)));
    }

    

    [RelayCommand]
    async Task SaveAsync()
    {
        if (String.IsNullOrWhiteSpace(ItemText.Name))
            return;
        else
        {
            if (ItemText.StartDate < DateTime.Now) await Task.FromResult(ItemText.StartDate = null);
            if (ItemText.DueDate < DateTime.Now) await Task.FromResult(ItemText.DueDate = null);
            CurrentItem.Clone(ItemText);
            //CurrentItem.Weather.Forcast = await Task.FromResult(ForecastSelection == 0 ? String.Empty : ForecastOptions[ForecastSelection].ToString());
            //CurrentItem.Weather.High = await Task.FromResult(HighTempSelection == 0 ? String.Empty : TempRange[HighTempSelection].ToString());
            //CurrentItem.Weather.Forcast = await Task.FromResult(LowTempSelection == 0 ? String.Empty : TempRange[LowTempSelection].ToString());
            //CurrentItem.Weather.Forcast = await Task.FromResult(WindSpeedSelection == 0 ? String.Empty : WindSpeeds[WindSpeedSelection].ToString());
            await dataService.UpdateItemAsync(CurrentItem);
            await Task.Delay(100);
            await Shell.Current.GoToAsync("..", true);
        }
    }

    [RelayCommand]
    async Task DeleteAsync()
    {
        if (CurrentItem.Id != 0)
        {
            await dataService.DeleteItemAsync(CurrentItem);
            CurrentItem.Id = 0;
        }
        await Task.Delay(100);
        await Shell.Current.GoToAsync("..", true);
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
