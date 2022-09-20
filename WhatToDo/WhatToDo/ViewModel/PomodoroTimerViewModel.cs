

namespace WhatToDo.ViewModel;

public partial class PomodoroTimerViewModel : BaseViewModel
{

    ISessionService session;


    public PomodoroTimerViewModel(ISessionService session)
    {
        this.session = session;
    }

}
