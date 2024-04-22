namespace AD.FsCheck.MSTest.Tests;

#pragma warning disable IDE0060 // Remove unused parameter

[Properties(MaxTest = ClassMaxTest)]
public sealed class ClassAndMethodMaxTest : CommandLineTest
{
    const int ClassMaxTest = 20;
    const int MethodMaxTest = 30;

    public ClassAndMethodMaxTest() : base(nameof(ClassAndMethodMaxTest))
    { }

    [CommandLineProperty]
    public void Class_is_inherited(int a)
    { }

    [TestMethod]
    public async Task Class_is_inherited_test() => AreEqual(ClassMaxTest, await AssertSuccess(nameof(Class_is_inherited)));

    [CommandLineProperty(MaxTest = MethodMaxTest)]
    public void Method_overrides_class(int a)
    { }

    [TestMethod]
    public async Task Method_overrides_class_test() => AreEqual(MethodMaxTest, await AssertSuccess(nameof(Method_overrides_class)));
}

#pragma warning restore IDE0060 // Remove unused parameter
