

namespace WhatToDo.Views.Pages;

public partial class CustomListPage : ContentPage
{
	public CustomListPage(CustomListViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}