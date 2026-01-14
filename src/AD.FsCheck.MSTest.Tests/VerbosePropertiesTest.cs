namespace AD.FsCheck.MSTest.Tests;

[Properties(MaxTest = MaxTest, Verbose = true)]
public sealed class VerbosePropertiesTest : CommandLineTest
{
    const int MaxTest = 20;

    public VerbosePropertiesTest() : base(nameof(VerbosePropertiesTest))
    { }

    [CommandLineProperty]
    public void Verbose(int a, int b) => AreEqual(a + b, b + a);

    [TestMethod]
    public async Task Verbose_test() => await Test(nameof(Verbose), MaxTest + 1);

    [CommandLineProperty(Verbose = false)]
    public void NotVerbose(int a, int b) => AreEqual(a + b, b + a);

    [TestMethod]
    public async Task NotVerbose_test() => await Test(nameof(NotVerbose), MaxTest + 1);
    //when 'Verbose' is set to true for the class, it cannot be set to false for a single property

    async Task Test(string testName, int expectedLines)
    {
        var result = await Run(testName, Fetch.StdOut);
        var lines = result.Split(Environment.NewLine);
        HasCount(expectedLines, lines);
    }
}
