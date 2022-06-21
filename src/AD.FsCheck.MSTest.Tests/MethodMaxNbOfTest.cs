namespace AD.FsCheck.MSTest.Tests;

#pragma warning disable IDE0060 // Remove unused parameter

[TestClass]
public class MethodMaxNbOfTest : CommandLineTest
{
    public MethodMaxNbOfTest() : base(nameof(MethodMaxNbOfTest))
    { }

    const int MaxNbOfTest = 30;

    [CommandLine(true), Property(MaxNbOfTest = MaxNbOfTest)]
    public void Method(int a)
    { }

    [CommandLine(false), TestMethod]
    public async Task Method_test() => Assert.AreEqual(MaxNbOfTest, await AssertSuccess(nameof(Method)));
}

#pragma warning restore IDE0060 // Remove unused parameter
