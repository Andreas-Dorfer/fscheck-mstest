namespace AD.FsCheck.MSTest.Tests;

[Properties(Replay = "17,42")]
public class ReplayPrecedenceTest : CommandLineTest
{
    public ReplayPrecedenceTest() : base(nameof(ReplayPrecedenceTest))
    { }

    [CommandLineProperty(Replay = "1561428431,297099475")]
    public void Prop(int a) => IsTrue(a < 17);

    [TestMethod]
    public async Task Propt_test()
    {
        var msg = await Run(nameof(Prop), Fetch.StdErr);
        IsTrue(msg.StartsWith("Falsifiable, after 28 tests (1 shrink) (StdGen (1561428431,297099475))"));
    }
}
