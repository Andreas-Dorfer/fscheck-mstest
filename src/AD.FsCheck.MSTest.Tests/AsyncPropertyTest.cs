namespace AD.FsCheck.MSTest.Tests;

#pragma warning disable IDE0060 // Remove unused parameter

[TestClass]
public class AsyncPropertyTest : CommandLineTest
{
    public AsyncPropertyTest() : base(nameof(AsyncPropertyTest))
    { }

    [CommandLineProperty]
    public async Task Async_property(int a) => await Task.Delay(10);

    [TestMethod]
    public async Task Async_property_test() => await AssertSuccess(nameof(Async_property));


    [CommandLineProperty]
    public async Task Failing_async_property(int a)
    {
        await Task.Delay(10);
        Fail();
    }

    [TestMethod]
    public async Task Failing_async_property_test() => await AssertFalsifiable(nameof(Failing_async_property));
}

#pragma warning restore IDE0060 // Remove unused parameter
