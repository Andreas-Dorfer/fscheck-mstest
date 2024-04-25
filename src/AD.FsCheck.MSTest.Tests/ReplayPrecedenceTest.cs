namespace AD.FsCheck.MSTest.Tests;

[Properties(Replay = "17,42")]
public sealed class ReplayPrecedenceTest : CommandLineTest
{
    public ReplayPrecedenceTest() : base(nameof(ReplayPrecedenceTest))
    { }

    [CommandLineProperty(Replay = "5195330141687306492,9724345478383734501")]
    public void Prop(int a) => IsTrue(a < 17);

    [TestMethod]
    public async Task Prop_test()
    {
        var msg = await Run(nameof(Prop), Fetch.StdErr);
        IsTrue(msg.StartsWith("Falsifiable, after 21 tests (2 shrinks) (5195330141687306492,9724345478383734501)"));
    }
}
