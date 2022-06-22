using FsCheck;

namespace AD.FsCheck.MSTest.Tests;

#pragma warning disable IDE0060 // Remove unused parameter

[TestClass]
public class FailedTest : CommandLineTest
{
    public FailedTest() : base(nameof(FailedTest))
    { }

    [Property]
    public void Failing_property(int a)
    {
        Assert.Fail();
    }

    [TestMethod]
    public void Foo()
    {
        MSTestRunner runner = new();
        Configuration config = new()
        {
            Runner = runner
        };
        Prop.ForAll<int>(a => Assert.Fail()).Check(config);
    }
}

#pragma warning restore IDE0060 // Remove unused parameter
