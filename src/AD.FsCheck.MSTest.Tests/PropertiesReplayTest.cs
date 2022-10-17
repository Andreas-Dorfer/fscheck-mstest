namespace AD.FsCheck.MSTest.Tests;

[Properties(Replay = "1561428431,297099475")]
public class PropertiesReplayTest : CommandLineTest
{
    public PropertiesReplayTest() : base(nameof(PropertiesReplayTest))
    { }

    [CommandLineProperty]
    public void Prop(int a) => IsTrue(a < 17);

    [TestMethod]
    public async Task Prop_test()
    {
        var msg = await Run(nameof(Prop), Fetch.StdErr);
        IsTrue(msg.StartsWith("Falsifiable, after 28 tests (1 shrink) (StdGen (1561428431,297099475))"));
    }
}
