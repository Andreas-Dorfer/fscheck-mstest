namespace AD.FsCheck.MSTest.Tests;

#pragma warning disable IDE0060 // Remove unused parameter

[TestClass]
public sealed class MethodMaxTest : CommandLineTest
{
    const int MaxTest = 30;

    public MethodMaxTest() : base(nameof(MethodMaxTest))
    { }

    [CommandLineProperty(MaxTest = MaxTest)]
    public void Method(int a)
    { }

    [TestMethod]
    public async Task Method_test() => AreEqual(MaxTest, await AssertSuccess(nameof(Method)));
}

#pragma warning restore IDE0060 // Remove unused parameter
