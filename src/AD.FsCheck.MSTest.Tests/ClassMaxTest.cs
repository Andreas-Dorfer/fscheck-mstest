namespace AD.FsCheck.MSTest.Tests;

#pragma warning disable IDE0060 // Remove unused parameter

[Properties(MaxTest = MaxTest)]
public sealed class ClassMaxTest : CommandLineTest
{
    const int MaxTest = 20;

    public ClassMaxTest() : base(nameof(ClassMaxTest))
    { }

    [CommandLineProperty]
    public void Method(int a)
    { }

    [TestMethod]
    public async Task Method_test() => AreEqual(MaxTest, await AssertSuccess(nameof(Method)));
}

#pragma warning restore IDE0060 // Remove unused parameter
