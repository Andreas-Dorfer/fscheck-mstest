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
    { }

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
    { }

    [CommandLine(false), TestMethod]
    public async Task One_parameter_test() => await AssertSuccess(nameof(One_parameter));

    #endregion

    #region Two

    [CommandLine(true), Property]
    public void Two_parameters(int a, int b)
    { }

    [CommandLine(false), TestMethod]
    public async Task Two_parameters_test() => await AssertSuccess(nameof(Two_parameters));

    #endregion

    #region Three

    [CommandLine(true), Property]
    public void Three_parameters(int a, int b, int c)
    { }

    [CommandLine(false), TestMethod]
    public async Task Three_parameters_test() => await AssertSuccess(nameof(Three_parameters));

    #endregion

    #region Four

    [CommandLine(true), Property]
    public void Four_parameters(int a, int b, int c, int d)
    { }

    [CommandLine(false), TestMethod]
    public async Task Four_parameters_test() => await AssertSuccess(nameof(Four_parameters));

    #endregion

    #region Five

    [CommandLine(true), Property]
    public void Five_parameters(int a, int b, int c, int d, int e)
    { }

    [CommandLine(false), TestMethod]
    public async Task Five_parameters_test() => await AssertSuccess(nameof(Five_parameters));

    #endregion

    #region Six

    [CommandLine(true), Property]
    public void Six_parameters(int a, int b, int c, int d, int e, int f)
    { }

    [CommandLine(false), TestMethod]
    public async Task Six_parameters_test() => await AssertSuccess(nameof(Six_parameters));

    #endregion

    #region Seven

    [CommandLine(true), Property]
    public void Seven_parameters(int a, int b, int c, int d, int e, int f, int g)
    { }

    [CommandLine(false), TestMethod]
    public async Task Seven_parameters_test() => await AssertSuccess(nameof(Seven_parameters));

    #endregion

    #region Eight

    [CommandLine(true), Property]
    public void Eight_parameters(int a, int b, int c, int d, int e, int f, int g, int h)
    { }

    [CommandLine(false), TestMethod]
    public async Task Eight_parameters_test()
    {
        var errorMsg = await Run(nameof(Eight_parameters), Fetch.StdErr);
        Assert.AreEqual("The number of property parameters is limited to 7. The actual number is 8.", errorMsg);
    }

    #endregion
}

#pragma warning restore IDE0060 // Remove unused parameter
