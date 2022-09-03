using System.Collections.ObjectModel;

namespace WhatToDoTests;

public class MainViewModel_Tests : IClassFixture<MainViewModel_Fixture>
{
    private MainViewModel_Fixture fixture;

    public MainViewModel_Tests(MainViewModel_Fixture fixture)
    {
        this.fixture = fixture;
    }

    #region ViewModel Instantiation
    [Fact]
    public void CreateViewModel_Should_Exist()
    {
        //Arrange

        //Act

        //Assert
        fixture.SUT
            .Should().NotBeNull()
            .And.BeOfType<MainViewModel>();
        fixture.SUT.Title
            .Should().Be("WhatToDo");
        fixture.SUT.SelectedItem
            .Should().NotBeNull()
            .And.BeOfType<ToDoItem>()
            .And.BeEquivalentTo(new ToDoItem());
        fixture.SUT.Items
            .Should().NotBeNull()
            .And.BeOfType<ObservableCollection<ToDoItem>>()
            .And.HaveCount(0);

    }
    #endregion ViewModel Instantiation

    #region Items Property

    [Fact]
    public void ItemsObservableCollection_Should_NotifyChange_AddedTo()
    {
        //Arrange
        List<ToDoItem> items = new() { new ToDoItem() };
        fixture.SUT.Items.AddRange(items);
        using var monitor = fixture.SUT.Monitor();

        //Act
        fixture.SUT.Items.Add(new ToDoItem());

        //Assert
        fixture.SUT.Items
            .Should().NotBeNull()
            .And.HaveCount(2);
        monitor.Should().RaisePropertyChangeFor(x => x.Items[1]);

        //monitor.Should().Raise("OnCollectionChanged");
    }
    #endregion Items Property

}