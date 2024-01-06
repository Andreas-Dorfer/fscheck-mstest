namespace AD.FsCheck.MSTest.Tests;

[Properties(MaxNbOfTest = MaxNbOfTest)]
public sealed class VerboseTest : CommandLineTest
{
    const int MaxNbOfTest = 20;

    public VerboseTest() : base(nameof(VerboseTest))
    { }

    [CommandLineProperty(Verbose = true)]
    public void Verbose(int a, int b) => AreEqual(a + b, b + a);

    [TestMethod]
    public async Task Verbose_test() => await Test(nameof(Verbose), MaxNbOfTest + 1);

    [CommandLineProperty]
    public void NotVerbose(int a, int b) => AreEqual(a + b, b + a);

    [TestMethod]
    public async Task NotVerbose_test() => await Test(nameof(NotVerbose), 1);

    async Task Test(string testName, int expectedLines)
    {
        var result = await Run(testName, Fetch.StdOut);
        var lines = result.Split(Environment.NewLine);
        AreEqual(expectedLines, lines.Length);
    }
}
