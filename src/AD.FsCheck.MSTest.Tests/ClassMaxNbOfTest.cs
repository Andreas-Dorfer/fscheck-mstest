namespace AD.FsCheck.MSTest.Tests;

#pragma warning disable IDE0060 // Remove unused parameter

[Properties(MaxNbOfTest = MaxNbOfTest)]
public class ClassMaxNbOfTest : CommandLineTest
{
    const int MaxNbOfTest = 20;

    public ClassMaxNbOfTest() : base(nameof(ClassMaxNbOfTest))
    { }

    [CommandLineProperty]
    public void Method(int a)
    { }

    [TestMethod]
    public async Task Method_test() => Assert.AreEqual(MaxNbOfTest, await AssertSuccess(nameof(Method)));
}

#pragma warning restore IDE0060 // Remove unused parameter
