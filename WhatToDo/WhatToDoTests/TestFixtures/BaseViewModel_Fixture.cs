using FluentAssertions;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography.X509Certificates;
using WhatToDo.ViewModel;

namespace WhatToDoTests.TestFixtures;


public class BaseViewModel_Fixture<T> : Fixture<T> where T : class
{
    public bool Initial { get; set; } = false;
    public bool Altered { get; set; } = false;

    public BaseViewModel_Fixture()
    {
        if (typeof(T) == typeof(BaseViewModel))
        {
            SUT = (T)Convert.ChangeType(new BaseViewModel(), typeof(T));
        }
    }
    public override void InitializeFixture()
    {

    }

    public override void LoadTestData()
    {
        Initial = Altered = false;
    }
}