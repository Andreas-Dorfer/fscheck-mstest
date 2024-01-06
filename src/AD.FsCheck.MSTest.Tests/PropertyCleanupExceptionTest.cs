namespace AD.FsCheck.MSTest.Tests;

[TestClass]
public sealed class PropertyCleanupExceptionTest : CommandLineTest
{
    public PropertyCleanupExceptionTest() : base(nameof(PropertyCleanupExceptionTest))
    { }

    [PropertyCleanup]
    public static void PropertyCleanup(string propName)
    {
        AreEqual(nameof(Prop), propName);
        Fail(nameof(PropertyCleanup));
    }

    [CommandLineProperty]
    public void Prop(int _)
    { }

    [TestMethod]
    public async Task Prop_test() => await AssertMessage(nameof(Prop), nameof(PropertyCleanup));
}
