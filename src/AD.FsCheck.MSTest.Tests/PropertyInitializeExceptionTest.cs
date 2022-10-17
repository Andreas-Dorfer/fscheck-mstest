namespace AD.FsCheck.MSTest.Tests;

[TestClass]
public class PropertyInitializeExceptionTest : CommandLineTest
{
    public PropertyInitializeExceptionTest() : base(nameof(PropertyInitializeExceptionTest))
    { }

    [PropertyInitialize]
    public static void PropertyInitialize() => Fail(nameof(PropertyInitialize));

    [CommandLineProperty]
    public void Prop(int _) => Fail("property should not be invoked");

    [TestMethod]
    public async Task Prop_test() => await AssertMessage(nameof(Prop), nameof(PropertyInitialize));
}
