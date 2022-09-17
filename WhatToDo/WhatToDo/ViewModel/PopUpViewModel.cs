
//using IntelliJ.Lang.Annotations;
using WhatToDo.Service;
//using static Android.Content.ClipData;
//using static Android.Icu.Text.CaseMap;

namespace WhatToDo.ViewModel;

public partial class PopUpViewModel<T> : BaseViewModel where T : IModelObject<T>
{
    [ObservableProperty]
    T input;

    [ObservableProperty]
    T displayed;

    [RelayCommand]
    async Task UpdateAsync()
    {
        if (Input is not null || Displayed is not null || !HelperFunctions.Compare(Input, Displayed))
        {
            Input.CopyFrom(Displayed);
            IsRefreshing = true;
            this.Dispose();
        }
    }

    [RelayCommand]
    async Task CancelAsync()
    {
        await Task.FromResult(() => {
            IsRefreshing = true;
            this.Dispose();
        });
    }

    public override void Dispose()
    {
        base.Dispose();
        input = default;
        displayed = default;
    }
}
