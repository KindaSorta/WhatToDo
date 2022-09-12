

using CommunityToolkit.Mvvm.Messaging.Messages;

namespace WhatToDo.Messages;

public class DeleteMessage : ValueChangedMessage<ToDoItem>
{
    public DeleteMessage(ToDoItem value) : base(value) { }
}
