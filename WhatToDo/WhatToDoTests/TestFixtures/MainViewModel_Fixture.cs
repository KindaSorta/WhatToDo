using FluentAssertions;
using Moq;
using System.Security.Cryptography.X509Certificates;
using WhatToDo.Service;
using WhatToDo.ViewModel;

namespace WhatToDoTests.TestFixtures;

public class MainViewModel_Fixture : BaseViewModel_Fixture<MainViewModel>, IDisposable
{
    public Mock<IDataService> DataServiceMock { get; private set; } = new();

    public MainViewModel_Fixture() 
    {
        //SUT = new MainViewModel(DataServiceMock.Object);
    }

    public override void InitializeFixture()
    {

    }

    public override void LoadTestData()
    {
        base.LoadTestData();
    }

    public void Dispose() { }
}