namespace AD.FsCheck.MSTest.Tests.Arbitraries;

[TestClass]
public sealed class From100To200Tests
{
    [Property]
    public void IsInRange(From100To200 x)
    {
        Assert.IsTrue(x.Value >= 100);
        Assert.IsTrue(x.Value <= 200);
    }
}
