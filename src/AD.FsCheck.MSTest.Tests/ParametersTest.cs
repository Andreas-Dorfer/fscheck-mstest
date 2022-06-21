namespace AD.FsCheck.MSTest.Tests;

[TestClass]
public class ParametersTest : CommandLineTest
{
    public ParametersTest() : base(nameof(ParametersTest))
    { }

    [CommandLine(true), Property]
    public void One_parameter(int a)
    {

    }

    [CommandLine(false), TestMethod]
    public async Task One_parameter_test() => await AssertSuccess(nameof(One_parameter));
}
