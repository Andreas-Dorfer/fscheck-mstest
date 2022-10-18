namespace AD.FsCheck.MSTest.Tests;

[Properties(QuietOnSuccess = true)]
public class QuietOnSuccessPropertiesTest : CommandLineTest
{
    public QuietOnSuccessPropertiesTest() : base(nameof(QuietOnSuccessPropertiesTest))
    { }

    [CommandLineProperty]
    public void True(int _)
    { }

    [TestMethod]
    public async Task True_test() => await AssertIsQuiet(nameof(True));

    [CommandLineProperty(QuietOnSuccess = false)]
    public void NotQuiet(int _)
    { }

    [TestMethod]
    public async Task NotQuiet_test() => await AssertIsQuiet(nameof(NotQuiet));
    //when 'QuietOnSuccess' is set to true for the class, it cannot be set to false for a single property

    async Task AssertIsQuiet(string testName)
    {
        try
        {
            var result = await Run(testName, Fetch.StdOut);
        }
        catch (InvalidOperationException ex)
        {
            AreEqual("Sequence contains no elements", ex.Message);
        }
    }
}
