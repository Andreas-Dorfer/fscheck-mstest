namespace AD.FsCheck.MSTest.Tests;

[TestClass]
public sealed class AsyncLocalInitializeTest
{
    static readonly AsyncLocal<Guid> propId = new();

    [PropertyInitialize]
    public static void PropertyInitialize(string propName)
    {
        CollectionAssert.Contains(new[] { nameof(PropA), nameof(PropB) }, propName);
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
