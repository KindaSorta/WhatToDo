

namespace WhatToDo.Template;

public partial class TodoItemInputField : ContentView
{
	public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(
		nameof(LabelText), typeof(string), typeof(TodoItemInputField), string.Empty);
    public static readonly BindableProperty ItemFieldProperty = BindableProperty.Create(
        nameof(ItemField), typeof(string), typeof(TodoItemInputField), string.Empty);

    public string LabelText
    {
        get => (string)GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }

    public string ItemField
    {
        get => (string)GetValue(ItemFieldProperty);
        set => SetValue(ItemFieldProperty, value);
    }

    public TodoItemInputField()
	{
		InitializeComponent();
	}
}