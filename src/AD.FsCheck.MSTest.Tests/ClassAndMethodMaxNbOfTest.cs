namespace AD.FsCheck.MSTest.Tests;

#pragma warning disable IDE0060 // Remove unused parameter

[Properties(MaxNbOfTest = ClassMaxNbOfTest)]
public class ClassAndMethodMaxNbOfTest : CommandLineTest
{
    const int ClassMaxNbOfTest = 20;
    const int MethodMaxNbOfTest = 30;

    public ClassAndMethodMaxNbOfTest() : base(nameof(ClassAndMethodMaxNbOfTest))
    { }

    [CommandLineProperty]
    public void Class_is_inherited(int a)
    { }

    [TestMethod]
    public async Task Class_is_inherited_test() => Assert.AreEqual(ClassMaxNbOfTest, await AssertSuccess(nameof(Class_is_inherited)));

    [CommandLineProperty(MaxNbOfTest = MethodMaxNbOfTest)]
    public void Method_overrides_class(int a)
    { }

    [TestMethod]
    public async Task Method_overrides_class_test() => Assert.AreEqual(MethodMaxNbOfTest, await AssertSuccess(nameof(Method_overrides_class)));
}

#pragma warning restore IDE0060 // Remove unused parameter
