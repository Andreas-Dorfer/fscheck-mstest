namespace AD.FsCheck.MSTest.Tests;

[TestClass]
public class QuietOnSuccessTest : CommandLineTest
{
    public QuietOnSuccessTest() : base(nameof(QuietOnSuccessTest))
    { }

    [CommandLineProperty(QuietOnSuccess = true)]
    public void True(int _)
    { }

    [TestMethod]
    public async Task True_Test()
    {
        try
        {
            var result = await Run(nameof(True), Fetch.StdOut);
        }
        catch (InvalidOperationException ex)
        {
            AreEqual("Sequence contains no elements", ex.Message);
        }
    }
}
