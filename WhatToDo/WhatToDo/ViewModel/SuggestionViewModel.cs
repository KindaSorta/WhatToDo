

namespace WhatToDo.ViewModel;


public partial class SuggestionViewModel : BaseViewModel
{

    ISessionService session;


    public SuggestionViewModel(ISessionService session)
    {
        this.session = session;
    }

}
