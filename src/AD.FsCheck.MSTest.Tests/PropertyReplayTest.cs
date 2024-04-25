namespace AD.FsCheck.MSTest.Tests;

[TestClass]
public sealed class PropertyReplayTest : CommandLineTest
{
    public PropertyReplayTest() : base(nameof(PropertyReplayTest))
    { }

    [CommandLineProperty(Replay = "1358818329932720880,1613334182980143787")]
    public void Prop1(int a) => IsTrue(a < 17);

    [TestMethod]
    public async Task Prop1_test() => await Run(nameof(Prop1));

    [CommandLineProperty(Replay = " 1358818329932720880   , 1613334182980143787 ")]
    public void Prop2(int a) => IsTrue(a < 17);

    [TestMethod]
    public async Task Prop2_test() => await Run(nameof(Prop2));

    [CommandLineProperty(Replay = "(1358818329932720880,1613334182980143787)")]
    public void Prop3(int a) => IsTrue(a < 17);

    [TestMethod]
    public async Task Prop3_test() => await Run(nameof(Prop3));

    async Task Run(string testName)
    {
        var msg = await Run(testName, Fetch.StdErr);
        IsTrue(msg.StartsWith("Falsifiable, after 25 tests (3 shrinks) (1358818329932720880,1613334182980143787)"));
    }
}
