using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using WhatToDo.ViewModel;

namespace WhatToDoTests.TestFixtures;

public abstract class TestFixture<T> where T : class
{
    public IFixture Fixture { get; protected set; } = new Fixture().Customize(new AutoMoqCustomization());

    private T _sut = default!;
    public T SUT
    {
        get => _sut ?? throw new ArgumentException();
        protected set => _sut = value ?? throw new ArgumentNullException();
    }

    public TestFixture() 
    {
        _sut = Fixture.Create<T>();
        InitializeFixture();
    }

    public virtual void InitializeFixture()
    {
        LoadTestData();
    }

    public abstract void LoadTestData();
}