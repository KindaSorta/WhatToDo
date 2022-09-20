using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Moq;
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
            var sut = fixture.Build<StartViewModel>().OmitAutoProperties().Create();
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
            var sut = fixture.Build<StartViewModel>().OmitAutoProperties().Create();
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



    #region Refresh

    #endregion Refresh  

    #region GoToDetails

    #endregion GoToDetails

    #region GetData

    #region Intial Data Load

    #endregion Initial Data Load

    #region Get Items

    [Theory, AutoMoqData]
    public async Task GetData_AppIsBusy_ShouldNot_ReachDataService([Frozen] Mock<IDataService> dataServiceMock, [NoAutoProperties] MainViewModel sut)
    {
        //Arrange
        sut.IsBusy = true;

        //Act
        await sut.GetItemsCommand.ExecuteAsync(null);

        //Assert
        sut.IsBusy.Should().BeTrue();
        dataServiceMock.Verify(_ => _.GetItemsAsync(), Times.AtMostOnce); // Runs once OnAppear due to refresh

    }

/*    [Theory, AutoMoqData]
    public async Task GetData_ServiceThrowsException_Should_GetAlert([Frozen]Mock<IDataService> dataServiceMock, [Frozen]Mock<IDialogService> dialogeMock, StartViewModel sut, Exception exception)
    {
        //Arrange
        sut.IsBusy = false;
        dialogeMock.Setup(_ => _.DisplayAlert(It.IsAny<string>(), exception.Message, It.IsAny<string>()));
            
        dataServiceMock.Setup(_ => _.GetItemsAsync())
            .ThrowsAsync(exception);

        //Act
        await sut.GetItemsCommand.ExecuteAsync(null);

        //Assert
        sut.Items.Should().BeEmpty();
        sut.IsBusy.Should().BeFalse();
        dataServiceMock.Verify(_ => _.GetItemsAsync(), Times.Exactly(2));
        dialogeMock.Verify(_ => _.DisplayAlert(
            It.IsAny<string>(), exception.Message, It.IsAny<string>(), null), 
            Times.Once);
    }*/

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
        dataServiceMock.Verify(_ => _.GetItemsAsync(), Times.Exactly(2));
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
        dataServiceMock.Verify(_ => _.GetItemsAsync(), Times.Exactly(2));
        dialogeMock.Verify(_ => _.DisplayAlert(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null),
            Times.Never);
    }

    #endregion Get Items

    #region Get Geoloation

    #endregion Get Geolocation

    #region Get Weather

    #endregion Get Weather

    #endregion GetData

    #region Filtering

    #region Filter Selector

    #endregion Filter Selector

    #region Display Completed

    #endregion Display Completed

    #region Display Incomplete

    #endregion Display Incomplete

    #region Format Display Objects

    #endregion Format Display Objects

    #region Filter Query Items


    [Theory, AutoMoqData]
    public async Task FilterQuery_Should_ReturnList([NoAutoProperties] MainViewModel sut, IEnumerable<ToDoItem> items)
    {
        //Arrange

        //Act
        var result = await sut.FilterQueryItems(items);

        //Assert
        result.Should().BeOfType<List<ToDoItem>>();
        result.Should().HaveCount(3);   
    }


    #endregion Filter Query Items

    #endregion Filtering


    #region Item Edits

    #region AddItem

    #endregion AddItem

    #region TapItem

    #endregion TapItem

    #region Toggle Complete

    #endregion Toggle Complete

    #region DeleteItem

    #endregion DeleteItem

    #endregion Item Edits



    #region Dispose

    #endregion Dispose
}