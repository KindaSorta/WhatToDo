namespace WhatToDo.Views.Pages;

public partial class SuggestionPage : ContentPage
{
	public SuggestionPage(SuggestionViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}