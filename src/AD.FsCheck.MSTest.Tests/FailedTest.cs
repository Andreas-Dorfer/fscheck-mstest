namespace AD.FsCheck.MSTest.Tests;

#pragma warning disable IDE0060 // Remove unused parameter

[TestClass]
public class FailedTest : CommandLineTest
{
    public FailedTest() : base(nameof(FailedTest))
    { }

    [CommandLineProperty]
    public void Failing_property(int a) => Assert.Fail();

    [TestMethod]
    public async Task Failing_property_test()
    {
        var errorMsg = await Run(nameof(Failing_property), Fetch.StdErr);
        Assert.IsTrue(errorMsg.StartsWith("Falsifiable,"));
    }
}

#pragma warning restore IDE0060 // Remove unused parameter
