using WhatToDo.Model;
using WhatToDo.Service;

namespace WhatToDo.View;

public partial class MainPage : ContentPage
{

    public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
	}

}

