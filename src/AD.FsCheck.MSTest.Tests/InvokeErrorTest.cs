namespace AD.FsCheck.MSTest.Tests;

public struct NotGeneratable
{
}

[TestClass]
public sealed class InvokeErrorTest : CommandLineTest
{
    public InvokeErrorTest() : base(nameof(InvokeErrorTest))
    { }

    [CommandLineProperty]
    public void CannotGenerate(NotGeneratable _)
    { }

    [TestMethod]
    public async Task CannotGenerate_test()
    {
        var msg = await Run(nameof(CannotGenerate), Fetch.StdErr);
        AreEqual($"The type {typeof(NotGeneratable).FullName} is not handled automatically by FsCheck. Consider using another type or writing a generator for it.",
            msg);
    }
}
