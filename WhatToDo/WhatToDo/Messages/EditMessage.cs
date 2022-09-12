

using CommunityToolkit.Mvvm.Messaging.Messages;

namespace WhatToDo.Messages;

public class EditMessage : ValueChangedMessage<ToDoItem>
{
    public EditMessage(ToDoItem value) : base(value) { }
}
