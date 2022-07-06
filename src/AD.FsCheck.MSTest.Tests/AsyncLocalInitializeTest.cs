namespace AD.FsCheck.MSTest.Tests;

[TestClass]
public class AsyncLocalInitializeTest
{
    static readonly AsyncLocal<Guid> propId = new AsyncLocal<Guid>();

    [PropertyInitialize]
    public static void PropertyInitialize()
    {
        propId.Value = Guid.NewGuid();
    }

    [Property]
    public async Task PropA(int _)
    {
        var id = propId.Value;
        AreNotEqual(Guid.Empty, id);
        await Task.Delay(10).ConfigureAwait(false);
        AreEqual(id, propId.Value);
    }

    [Property]
    public async Task PropB(int _)
    {
        var id = propId.Value;
        AreNotEqual(Guid.Empty, id);
        await Task.Delay(10).ConfigureAwait(false);
        AreEqual(id, propId.Value);
    }
}
