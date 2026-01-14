namespace AD.FsCheck.MSTest.Tests.Arbitraries;

[TestClass]
public sealed class From100To200Tests
{
    [Property(Arbitrary = [typeof(Arbitraries)])]
    public void IsInRange(From100To200 x)
    {
        Assert.IsGreaterThanOrEqualTo(100, x.Value);
        Assert.IsLessThanOrEqualTo(200, x.Value);
    }
}
