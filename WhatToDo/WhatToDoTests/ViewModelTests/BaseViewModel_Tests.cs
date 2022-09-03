
using System.Security.Cryptography.X509Certificates;

namespace WhatToDoTests;

public class BaseViewModel_Tests : IClassFixture<BaseViewModel_Fixture<BaseViewModel>>
{
    private BaseViewModel_Fixture<BaseViewModel> fixture;

    public BaseViewModel_Tests(BaseViewModel_Fixture<BaseViewModel> fixture) 
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
        fixture.SUT.Should().NotBeNull();
        fixture.SUT.Should().BeOfType<BaseViewModel>();
    }
    #endregion ViewModel Instantiation

    #region Title Property
    [Fact]
    public void TitleObservableProperty_Should_NotifyChange()
    {
        //Arrange
        fixture.SUT.Title = "Initial";
        using var monitor = fixture.SUT.Monitor();

        //Act
        fixture.SUT.Title = "Altered";

        //Assert
        fixture.SUT.Title.Should().NotBeNull();
        fixture.SUT.Title.Should().BeOfType<string>();
        monitor.Should().RaisePropertyChangeFor(x => x.Title);
    }

    [Fact]
    public void TitleObservableProperty_ShouldNot_NotifyChange()
    {
        //Arrange
        fixture.SUT.Title = "Initial";
        using var monitor = fixture.SUT.Monitor();

        //Act
        fixture.SUT.Title = "Initial";

        //Assert
        fixture.SUT.Title.Should().NotBeNull();
        fixture.SUT.Title.Should().BeOfType<string>();
        monitor.Should().NotRaisePropertyChangeFor(x => x.Title);
    }
    #endregion Title Property

    #region Busy Properties
    [Fact]
    public void BusyObservableProperties_Should_NotifyChange()
    {
        //Arrange
        fixture.SUT.IsBusy = false;
        using var monitor = fixture.SUT.Monitor();

        //Act
        fixture.SUT.IsBusy = true;

        //Assert
        fixture.SUT.IsBusy.Should().BeTrue();
        monitor.Should().RaisePropertyChangeFor(x => x.IsBusy);
        monitor.Should().RaisePropertyChangeFor(x => x.IsNotBusy);
        fixture.SUT.IsBusy.Should().NotBe(fixture.SUT.IsNotBusy);
    }

    [Fact]
    public void BusyObservableProperties_ShouldNot_NotifyChange()
    {
        //Arrange
        fixture.SUT.IsBusy = true;
        using var monitor = fixture.SUT.Monitor();

        //Act
        fixture.SUT.IsBusy = true;

        //Assert
        fixture.SUT.IsBusy.Should().BeTrue();
        monitor.Should().NotRaisePropertyChangeFor(x => x.IsBusy);
        monitor.Should().NotRaisePropertyChangeFor(x => x.IsNotBusy);
        fixture.SUT.IsBusy.Should().NotBe(fixture.SUT.IsNotBusy);
    }
    #endregion Busy Properties

    #region Refreshing Property
    [Fact]
    public void IsRefreshingObservableProperty_Should_NotifyChange()
    {
        //Arrange
        fixture.SUT.IsRefreshing = false;
        using var monitor = fixture.SUT.Monitor();

        //Act
        fixture.SUT.IsRefreshing = true;

        //Assert
        fixture.SUT.IsRefreshing.Should().BeTrue();
        monitor.Should().RaisePropertyChangeFor(x => x.IsRefreshing);
    }

    [Fact]
    public void IsRefreshingObservableProperty_ShouldNot_NotifyChange()
    {
        //Arrange
        fixture.SUT.IsRefreshing = true;
        using var monitor = fixture.SUT.Monitor();

        //Act
        fixture.SUT.IsRefreshing = true;

        //Assert
        fixture.SUT.IsRefreshing.Should().BeTrue();
        monitor.Should().NotRaisePropertyChangeFor(x => x.IsRefreshing);
    }
    #endregion Refreshing Property
}