namespace AD.FsCheck.MSTest.Tests;

[TestClass]
public class DurationTest : CommandLineTest
{
    const int NbOfTest = 10;
    const int Yield = 200;

    public DurationTest() : base(nameof(DurationTest))
    { }

    [Property(MaxNbOfTest = NbOfTest)]
    public async Task Duration(int _) => await Task.Delay(Yield);

    [TestMethod]
    public async Task Duration_test()
    {
        var expected = TimeSpan.FromMilliseconds(NbOfTest * Yield);
        var actual = TimeSpan.Parse(await Run(nameof(Duration), Fetch.Duration));
        IsTrue(actual > expected);
        IsTrue(actual < expected * 1.2); //allow for up to 20% overhead
    }
}
