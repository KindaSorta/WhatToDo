using FluentAssertions;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using WhatToDo.ViewModel;

namespace WhatToDoTests.TestFixtures;

public abstract class Fixture<T> where T : class
{
    private T _sut = default!;
    public T SUT
    {
        get => (T)Convert.ChangeType(_sut, typeof(T)) ?? throw new ArgumentException();
        protected set => _sut = value ?? throw new ArgumentNullException();
    }

    public Fixture() 
    {
        InitializeFixture();
    }

    public virtual void InitializeFixture()
    {
        LoadTestData();
    }

    public abstract void LoadTestData();
}