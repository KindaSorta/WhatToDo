using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using System;
using System.Collections.ObjectModel;

namespace WhatToDoTests;

public class MainViewModel_Tests
{

    public MainViewModel_Tests() { }

    #region ViewModel Instantiation
    [Theory, AutoMoqData]
    public void CreateViewModel_Should_Exist_AutoMoq([NoAutoProperties] MainViewModel sut)
    {
        //Arrange

        //Act

        //Assert
        sut.Should().NotBeNull()
            .And.BeOfType<MainViewModel>();
        sut.Title
            .Should().Be("WhatToDo");
        sut.SelectedItem
            .Should().NotBeNull()
            .And.BeOfType<ToDoItem>()
            .And.BeEquivalentTo(new ToDoItem());
        sut.Items
            .Should().NotBeNull()
            .And.BeOfType<ObservableCollection<ToDoItem>>()
            .And.HaveCount(0);
    }
    #endregion ViewModel Instantiation

    // TODO: Figure out what is up with ObservableCollection Change Events
    #region Items Property
    /*
        [Fact]
        public void ItemsObservableCollection_Should_NotifyChange_AddedTo()
        {
            //Arrange
            var sut = fixture.Build<MainViewModel>().OmitAutoProperties().Create();
            sut.Items.Add(fixture.Create<ToDoItem>());
            using var monitor = sut.Monitor();

            //Act
            sut.Items.Add(fixture.Create<ToDoItem>());

            //Assert
            sut.Items
                .Should().NotBeNull()
                .And.HaveCount(2);
            //monitor.Should().RaisePropertyChangeFor(x => x.Items);

            monitor.Should().Raise("OnCollectionChanged");
        }

        [Fact]
        public void ItemsObservableCollection_Should_NotifyChange_ItemEdited()
        {
            //Arrange
            var sut = fixture.Build<MainViewModel>().OmitAutoProperties().Create();
            sut.Items.AddRange(fixture.CreateMany<ToDoItem>());
            using var monitor = sut.Monitor();

            //Act
            ToDoItem edit = fixture.Create<ToDoItem>();
            sut.Items[0].Clone(edit);

            //Assert
            sut.Items
                .Should().NotBeNull();
            //monitor.Should().RaisePropertyChangeFor(x => x.Items[0]);

            monitor.Should().Raise("OnCollectionChanged");
        }*/
    #endregion Items Property

    #region SelectedItem Property
    [Theory, AutoMoqData]
    public void SelectedItemObservableProperty_Should_NotifyChange([NoAutoProperties] MainViewModel sut, ToDoItem item)
    {
        //Arrange
        using var monitor = sut.Monitor();

        //Act
        sut.SelectedItem = item;

        //Assert
        sut.SelectedItem.Should().NotBeNull();
        sut.SelectedItem.Should().BeOfType<ToDoItem>();
        monitor.Should().RaisePropertyChangeFor(x => x.SelectedItem);
    }

    [Theory, AutoMoqData]
    public void SelectedItemObservableProperty_ShouldNot_NotifyChange([NoAutoProperties] MainViewModel sut, ToDoItem item)
    {
        //Arrange
        sut.SelectedItem = item;
        using var monitor = sut.Monitor();

        //Act
        sut.SelectedItem = item;

        //Assert
        sut.SelectedItem.Should().NotBeNull();
        sut.SelectedItem.Should().BeOfType<ToDoItem>();
        monitor.Should().NotRaisePropertyChangeFor(x => x.SelectedItem);
    }
    #endregion SelectedItem Property

    #region Refresh

    #endregion Refresh  

    #region GoToDetails

    #endregion GoToDetails

    #region GetData
    [Theory, AutoMoqData]
    public async Task GetData_AppIsBusy_ShouldNot_ReachDataService([Frozen] Mock<IDataService> dataServiceMock, [NoAutoProperties] MainViewModel sut)
    {
        //Arrange
        sut.IsBusy = true;

        //Act
        await sut.GetItemsCommand.ExecuteAsync(null);

        //Assert
        sut.IsBusy.Should().BeTrue();
        dataServiceMock.Verify(_ => _.GetItemsAsync(), Times.Never);
    }

    [Theory, AutoMoqData]
    public async Task GetData_ServiceThrowsException_Should_GetAlert([Frozen]Mock<IDataService> dataServiceMock, [Frozen]Mock<IDialogService> dialogeMock, MainViewModel sut, Exception exception)
    {
        //Arrange
        sut.IsBusy = false;
        dataServiceMock.Setup(_ => _.GetItemsAsync())
            .ThrowsAsync(exception);

        //Act
        await sut.GetItemsCommand.ExecuteAsync(null);

        //Assert
        sut.Items.Should().BeEmpty();
        sut.IsBusy.Should().BeFalse();
        dataServiceMock.Verify(_ => _.GetItemsAsync(), Times.Once);
        dialogeMock.Verify(_ => _.DisplayAlert(
            It.IsAny<string>(), exception.Message, It.IsAny<string>(), null), 
            Times.Once);
    }

    [Theory, AutoMoqData]
    public async Task GetData_ServiceSucceds_Should_GetEmptyList([Frozen]Mock<IDataService> dataServiceMock, [Frozen]Mock<IDialogService> dialogeMock, MainViewModel sut)
    {
        //Arrange
        sut.IsBusy = false;
        dataServiceMock.Setup(_ => _.GetItemsAsync())
            .ReturnsAsync(new Dictionary<int, ToDoItem>());

        //Act
        await sut.GetItemsCommand.ExecuteAsync(null);

        //Assert
        sut.Items.Should().BeEmpty();
        sut.IsBusy.Should().BeFalse();
        dataServiceMock.Verify(_ => _.GetItemsAsync(), Times.Once);
        dialogeMock.Verify(_ => _.DisplayAlert(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null),
            Times.Never);
    }

    [Theory, AutoMoqData]
    public async Task GetData_ServiceSucceds_Should_GetsData([Frozen]Mock<IDataService> dataServiceMock, [Frozen] Mock<IDialogService> dialogeMock, MainViewModel sut, Dictionary<int, ToDoItem> data)
    {
        //Arrange
        sut.IsBusy = false;
        dataServiceMock.Setup(_ => _.GetItemsAsync())
            .ReturnsAsync(data);

        //Act
        await sut.GetItemsCommand.ExecuteAsync(null);

        //Assert
        sut.Items.Should().HaveCount(data.Count);
        sut.IsBusy.Should().BeFalse();
        dataServiceMock.Verify(_ => _.GetItemsAsync(), Times.Once);
        dialogeMock.Verify(_ => _.DisplayAlert(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null),
            Times.Never);
    }
    #endregion GetData

    #region AddItem

    #endregion AddItem

    #region TapItem

    #endregion TapItem

    #region DeleteItem

    #endregion DeleteItem
}