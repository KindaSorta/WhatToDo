
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using System.Security.Cryptography.X509Certificates;

namespace WhatToDoTests;

public class BaseViewModel_Tests
{
    public BaseViewModel_Tests() { }

    #region ViewModel Instantiation
    [Theory, AutoMoqData]
    public void CreateViewModel_Should_Exist([NoAutoProperties] BaseViewModel sut)
    {
        //Arrange

        //Act

        //Assert
        sut.Should().NotBeNull();
        sut.Should().BeOfType<BaseViewModel>();
        sut.Title.Should().Be(null);
        sut.IsBusy.Should().BeFalse();
        sut.IsRefreshing.Should().BeFalse();
    }
    #endregion ViewModel Instantiation

    #region Title Property
    [Theory, AutoMoqData]
    public void TitleObservableProperty_Should_NotifyChange([NoAutoProperties] BaseViewModel sut)
    {
        //Arrange
        sut.Title = "Initial";
        using var monitor = sut.Monitor();

        //Act
        sut.Title = "Altered";

        //Assert
        sut.Title.Should().NotBeNull();
        sut.Title.Should().BeOfType<string>();
        monitor.Should().RaisePropertyChangeFor(x => x.Title);
    }

    [Theory, AutoMoqData]
    public void TitleObservableProperty_ShouldNot_NotifyChange([NoAutoProperties] BaseViewModel sut)
    {
        //Arrange
        sut.Title = "Initial";
        using var monitor = sut.Monitor();

        //Act
        sut.Title = "Initial";

        //Assert
        sut.Title.Should().NotBeNull();
        sut.Title.Should().BeOfType<string>();
        monitor.Should().NotRaisePropertyChangeFor(x => x.Title);
    }
    #endregion Title Property

    #region Busy Properties
    [Theory, AutoMoqData]
    public void BusyObservableProperties_Should_NotifyChange([NoAutoProperties] BaseViewModel sut)
    {
        //Arrange
        sut.IsBusy = false;
        using var monitor = sut.Monitor();

        //Act
        sut.IsBusy = true;

        //Assert
        sut.IsBusy.Should().BeTrue();
        monitor.Should().RaisePropertyChangeFor(x => x.IsBusy);
        monitor.Should().RaisePropertyChangeFor(x => x.IsNotBusy);
        sut.IsBusy.Should().NotBe(sut.IsNotBusy);
    }

    [Theory, AutoMoqData]
    public void BusyObservableProperties_ShouldNot_NotifyChange([NoAutoProperties] BaseViewModel sut)
    {
        //Arrange
        sut.IsBusy = true;
        using var monitor = sut.Monitor();

        //Act
        sut.IsBusy = true;

        //Assert
        sut.IsBusy.Should().BeTrue();
        monitor.Should().NotRaisePropertyChangeFor(x => x.IsBusy);
        monitor.Should().NotRaisePropertyChangeFor(x => x.IsNotBusy);
        sut.IsBusy.Should().NotBe(sut.IsNotBusy);
    }
    #endregion Busy Properties

    #region Refreshing Property
    [Theory, AutoMoqData]
    public void IsRefreshingObservableProperty_Should_NotifyChange([NoAutoProperties] BaseViewModel sut)
    {
        //Arrange
        sut.IsRefreshing = false;
        using var monitor = sut.Monitor();

        //Act
        sut.IsRefreshing = true;

        //Assert
        sut.IsRefreshing.Should().BeTrue();
        monitor.Should().RaisePropertyChangeFor(x => x.IsRefreshing);
    }

    [Theory, AutoMoqData]
    public void IsRefreshingObservableProperty_ShouldNot_NotifyChange([NoAutoProperties] BaseViewModel sut)
    {
        //Arrange
        sut.IsRefreshing = true;
        using var monitor = sut.Monitor();

        //Act
        sut.IsRefreshing = true;

        //Assert
        sut.IsRefreshing.Should().BeTrue();
        monitor.Should().NotRaisePropertyChangeFor(x => x.IsRefreshing);
    }
    #endregion Refreshing Property
}