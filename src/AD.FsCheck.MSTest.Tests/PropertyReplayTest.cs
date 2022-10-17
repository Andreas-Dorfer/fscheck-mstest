namespace AD.FsCheck.MSTest.Tests;

[TestClass]
public class PropertyReplayTest : CommandLineTest
{
    public PropertyReplayTest() : base(nameof(PropertyReplayTest))
    { }

    [CommandLineProperty(Replay = "1561428431,297099475")]
    public void Prop1(int a) => IsTrue(a < 17);

    [TestMethod]
    public async Task Prop1_test() => await Run(nameof(Prop1));

    [CommandLineProperty(Replay = " 1561428431   , 297099475 ")]
    public void Prop2(int a) => IsTrue(a < 17);

    [TestMethod]
    public async Task Prop2_test() => await Run(nameof(Prop2));

    [CommandLineProperty(Replay = "(1561428431,297099475)")]
    public void Prop3(int a) => IsTrue(a < 17);

    [TestMethod]
    public async Task Prop3_test() => await Run(nameof(Prop3));

    async Task Run(string testName)
    {
        var msg = await Run(testName, Fetch.StdErr);
        IsTrue(msg.StartsWith("Falsifiable, after 28 tests (1 shrink) (StdGen (1561428431,297099475))"));
    }
}
