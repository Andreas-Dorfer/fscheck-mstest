namespace AD.FsCheck.MSTest.Tests;

[Properties(Replay = "10333802694858096737,13365262137747244545")]
public sealed class PropertiesReplayTest : CommandLineTest
{
    public PropertiesReplayTest() : base(nameof(PropertiesReplayTest))
    { }

    [CommandLineProperty]
    public void Prop(int a) => IsLessThan(17, a);

    [TestMethod]
    public async Task Prop_test()
    {
        var msg = await Run(nameof(Prop), Fetch.StdErr);
        StartsWith("Falsifiable, after 20 tests (2 shrinks) (10333802694858096737,13365262137747244545)", msg);
    }
}
