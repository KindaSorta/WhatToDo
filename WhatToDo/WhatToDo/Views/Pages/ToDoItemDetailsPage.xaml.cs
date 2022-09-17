
namespace WhatToDo.Views.Pages;

public partial class ToDoItemDetailsPage : ContentPage
{
	public ToDoItemDetailsPage(ToDoItemDetailsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}