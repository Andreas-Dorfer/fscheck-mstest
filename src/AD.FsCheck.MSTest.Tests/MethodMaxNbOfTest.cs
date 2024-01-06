namespace AD.FsCheck.MSTest.Tests;

#pragma warning disable IDE0060 // Remove unused parameter

[TestClass]
public sealed class MethodMaxNbOfTest : CommandLineTest
{
    const int MaxNbOfTest = 30;

    public MethodMaxNbOfTest() : base(nameof(MethodMaxNbOfTest))
    { }

    [CommandLineProperty(MaxNbOfTest = MaxNbOfTest)]
    public void Method(int a)
    { }

    [TestMethod]
    public async Task Method_test() => AreEqual(MaxNbOfTest, await AssertSuccess(nameof(Method)));
}

#pragma warning restore IDE0060 // Remove unused parameter
