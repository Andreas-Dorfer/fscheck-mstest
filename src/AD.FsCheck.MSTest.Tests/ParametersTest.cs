namespace AD.FsCheck.MSTest.Tests;

[TestClass]
public class ParametersTest : CommandLineTest
{
    public ParametersTest() : base(nameof(ParametersTest))
    { }

    #region Zero

    [CommandLine(true), Property]
    public void Zero_parameters()
    {

    }

    [CommandLine(false), TestMethod]
    public async Task Zero_parameters_test()
    {
        var errorMsg = await Run(nameof(Zero_parameters), Fetch.StdErr);
        Assert.AreEqual("Properties must have at least one parameter.", errorMsg);
    }

    #endregion

    #region One

    [CommandLine(true), Property]
    public void One_parameter(int a)
    {

    }

    [CommandLine(false), TestMethod]
    public async Task One_parameter_test() => await AssertSuccess(nameof(One_parameter));

    #endregion
}
