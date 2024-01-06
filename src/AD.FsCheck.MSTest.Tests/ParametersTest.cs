namespace AD.FsCheck.MSTest.Tests;

#pragma warning disable IDE0060 // Remove unused parameter

[TestClass]
public sealed class ParametersTest : CommandLineTest
{
    public ParametersTest() : base(nameof(ParametersTest))
    { }

    #region Zero

    [CommandLineProperty]
    public void Zero_parameters()
    { }

    [TestMethod]
    public async Task Zero_parameters_test()
    {
        var errorMsg = await Run(nameof(Zero_parameters), Fetch.StdErr);
        AreEqual("Properties must have at least one parameter.", errorMsg);
    }

    #endregion

    #region One

    [CommandLineProperty]
    public void One_parameter(int a)
    { }

    [TestMethod]
    public async Task One_parameter_test() => await AssertSuccess(nameof(One_parameter));

    #endregion

    #region Two

    [CommandLineProperty]
    public void Two_parameters(int a, int b)
    { }

    [TestMethod]
    public async Task Two_parameters_test() => await AssertSuccess(nameof(Two_parameters));

    #endregion

    #region Three

    [CommandLineProperty]
    public void Three_parameters(int a, int b, int c)
    { }

    [TestMethod]
    public async Task Three_parameters_test() => await AssertSuccess(nameof(Three_parameters));

    #endregion

    #region Four

    [CommandLineProperty]
    public void Four_parameters(int a, int b, int c, int d)
    { }

    [TestMethod]
    public async Task Four_parameters_test() => await AssertSuccess(nameof(Four_parameters));

    #endregion

    #region Five

    [CommandLineProperty]
    public void Five_parameters(int a, int b, int c, int d, int e)
    { }

    [TestMethod]
    public async Task Five_parameters_test() => await AssertSuccess(nameof(Five_parameters));

    #endregion

    #region Six

    [CommandLineProperty]
    public void Six_parameters(int a, int b, int c, int d, int e, int f)
    { }

    [TestMethod]
    public async Task Six_parameters_test() => await AssertSuccess(nameof(Six_parameters));

    #endregion

    #region Seven

    [CommandLineProperty]
    public void Seven_parameters(int a, int b, int c, int d, int e, int f, int g)
    { }

    [TestMethod]
    public async Task Seven_parameters_test() => await AssertSuccess(nameof(Seven_parameters));

    #endregion

    #region Eight

    [CommandLineProperty]
    public void Eight_parameters(int a, int b, int c, int d, int e, int f, int g, int h)
    { }

    [TestMethod]
    public async Task Eight_parameters_test()
    {
        var errorMsg = await Run(nameof(Eight_parameters), Fetch.StdErr);
        AreEqual("The number of property parameters is limited to 7. The actual number is 8.", errorMsg);
    }

    #endregion
}

#pragma warning restore IDE0060 // Remove unused parameter
