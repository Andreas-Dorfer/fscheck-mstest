namespace AD.FsCheck.MSTest.Tests;

[TestClass]
public sealed class ExceptionMessageTest : CommandLineTest
{
    static int count = 0;

    public ExceptionMessageTest() : base(nameof(ExceptionMessageTest))
    { }

    [CommandLineProperty]
    public void Failing_property(int _) => Fail($"{nameof(Failing_property)}: {Interlocked.Increment(ref count)}");

    [TestMethod]
    public async Task Failing_property_test() => await AssertFalsifiableWithMessage(nameof(Failing_property), $"{nameof(Failing_property)}: 1");
}
