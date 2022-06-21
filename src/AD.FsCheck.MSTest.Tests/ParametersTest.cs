namespace AD.FsCheck.MSTest.Tests;

#pragma warning disable IDE0060 // Remove unused parameter

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

    #region Two

    [CommandLine(true), Property]
    public void Two_parameters(int a, int b)
    {

    }

    [CommandLine(false), TestMethod]
    public async Task Two_parameters_test() => await AssertSuccess(nameof(Two_parameters));

    #endregion

    #region Three

    [CommandLine(true), Property]
    public void Three_parameters(int a, int b, int c)
    {

    }

    [CommandLine(false), TestMethod]
    public async Task Three_parameters_test() => await AssertSuccess(nameof(Three_parameters));

    #endregion
}

#pragma warning restore IDE0060 // Remove unused parameter
