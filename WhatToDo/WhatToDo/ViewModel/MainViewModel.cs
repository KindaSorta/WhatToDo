

using WhatToDo.Data;
using WhatToDo.Model;

namespace WhatToDo.ViewModel;

public partial class MainViewModel : BaseViewModel
{
    IDataService dataService;
    IDialogService dialogService;
    ISessionService session;

    public List<string> FilterOptions { get; } = FormOptions.FilterOptions;
    public List<string> CollectionOptions { get; } = FormOptions.TodoCollectionOptions;

    public ObservableCollection<ToDoItem> Items { get; } = new ();

    [ObservableProperty]
    string header;

    [ObservableProperty]
    string filter;

    [ObservableProperty]
    IList<IndexedToDoItem> displayItems;

    [ObservableProperty]
    bool isPopUp;

    [ObservableProperty]
    float blur;

    public MainViewModel(
        IDataService dataService, IDialogService dialogService, 
        ISessionService session)
    {
        this.dataService = dataService;
        this.dialogService = dialogService;
        this.session = session;
        Task.FromResult(InitialLoad());
    }


    #region Rendering

    public async Task InitialLoad()
    {
        if (Items == null || Items.Count == 0)
        {
            await GetItemsAsync();
        }

        Header = CollectionOptions[0];
        Filter = FilterOptions[2];
        Blur = 1;
        await Task.Delay(100);
        IsRefreshing = true;
    }

    [RelayCommand]
    async Task RefreshAsync()
    {
        //Get Todo Item Data
        if (Items == null || Items.Count == 0)
        {
            await GetItemsAsync();
        }
        else
        {
            if (session.ItemToEdit.Id == ObjectId.Empty)
            {
                Items.Remove(session.ItemToEdit);
            }
            session.ItemToEdit = new ToDoItem();
        }

        if (DisplayItems == null || Header == "Incomplete")
        {
            await IncompleteTasksAsync();
        }
        else
        {
            await CompletedTasksAsync();
        }
        
        IList<ToDoItem> items = await FilterQueryItems(DisplayItems.Select(x => x.BackToObject()));
        DisplayItems = null;
        DisplayItems = FormatDisplayObject(items.ToList());

        IsRefreshing = false;
    }

    #endregion Rendering


    #region Filtering

    public async Task<IList<ToDoItem>> FilterQueryItems(IEnumerable<ToDoItem> items)
    {
        // TODO: Make it handle nested search queries, and Tag filtering

        switch (Filter)
        {
            // Recent
            case var s when s == FilterOptions[1]:
                return await Task.FromResult(items.OrderByDescending(x => x.LastModifiedDate - DateTime.Now).ToList());

            // Upcoming
            case var s when s == FilterOptions[2]:
                return await Task.FromResult(items.OrderBy(x => x.DueDate == null).ThenBy(x => x.DueDate - DateTime.Now).ToList());

            // Priority
            case var s when s == FilterOptions[3]:
                return await Task.FromResult(items.OrderByDescending(x => x.Priority).ToList());

            // Urgency
            case var s when s == FilterOptions[4]:
                return await Task.FromResult(items.OrderBy(x => x.DueDate - DateTime.Now).OrderByDescending(x => x.Priority).ToList());

            // Suggested
            case var s when s == FilterOptions[5]:
                return await Task.FromResult(items.OrderBy(x => x.SuggestedDate.Count != 0 ? x.SuggestedDate[0] - DateTime.Now : x.DueDate - DateTime.Now).OrderByDescending(x => x.Priority).ToList());

            default:
                break;
        }
        return items.ToList();
    }

    [RelayCommand]
    void FilterSelector(string selection)
    {
        Filter = selection;
        IsPopUp = !IsPopUp;
        Blur = IsPopUp ? 0.4f : 1f;
        IsRefreshing = !IsPopUp;
    }

    [RelayCommand]
    async Task SearchAsync()
    {
        throw new NotImplementedException();
    }


    static IList<IndexedToDoItem> FormatDisplayObject(IList<ToDoItem> items)
    {
        IList<IndexedToDoItem> numberedList = new ObservableCollection<IndexedToDoItem>();
        if (items.Count > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                numberedList.Add(new IndexedToDoItem(i + 1, items[i]));
            }
        }
        return numberedList;
    }

    [RelayCommand]
    void ToggleIncompleteTasks(string selection)
    {
        Header = selection;
        IsRefreshing = true;
    }

    async Task CompletedTasksAsync()
    {
        IList<ToDoItem> items = await Task.FromResult(Items.Where(x => x.IsComplete).ToList());
        DisplayItems = FormatDisplayObject(items);
    }

    async Task IncompleteTasksAsync()
    {
        IList<ToDoItem> items = await Task.FromResult(Items.Where(x => x.NotComplete).ToList());
        DisplayItems = FormatDisplayObject(items);
    }

    #endregion Filtering


    #region Get Data


    [RelayCommand]
    async Task GetItemsAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;

            List<ToDoItem> data = await dataService.GetAllItems();

            if (data != null)
            {
                Items.Clear();
                foreach (var item in data)
                {
                    Items.Add(item);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get todo items: {ex.Message}");
            await dialogService.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false; 
        }
    }

    /*    public override async Task Cache()
        {
            await dataService.SaveSession(session);
            return;
        }*/

    #endregion Get Data


    #region Navigation

    async Task GoToDetails()
    {
        await Shell.Current.GoToAsync(nameof(ToDoItemDetailsPage), true);
    }

    #endregion Navigation


    #region ToDo Item Details / Edits


    [RelayCommand]
    async Task TapAsync(IndexedToDoItem item)
    {
        if (IsBusy) return;
        session.ItemToEdit = item.Value;
        await GoToDetails();
    }

    [RelayCommand]
    async Task AddAsync()
    {
        if (IsBusy) return;
        Items.Add(session.ItemToEdit);
        await GoToDetails();
    }

    [RelayCommand]
    async Task ToggoleCompleteAsync(IndexedToDoItem item)
    {
        if (IsBusy) return;
        item.Value.IsComplete = !item.Value.IsComplete;
        item.Value.LastModifiedDate = DateTime.Now;
        await dataService.SaveItem(item.Value);
        await Task.Delay(240);
        DisplayItems.Remove(item);
        IsRefreshing = true;
    }

    [RelayCommand]
    async Task DeleteAsync(IndexedToDoItem item)
    {
        if (IsBusy) return;

        bool answer = await dialogService.DisplayAlert("Alert!!", "You are about to Delete a Task! Please confirm...", "OK", "Cancel");
        if (!answer) return;

        await dataService.DeleteItem(item.Value);
        Items.Remove(item.Value);
        DisplayItems.Remove(item);
        IsRefreshing = true;
    }

    public override void Dispose() 
    {
        base.Dispose();
        
    }

    #endregion ToDo Item Details / Edits
}
