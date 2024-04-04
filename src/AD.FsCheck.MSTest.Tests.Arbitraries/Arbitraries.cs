namespace AD.FsCheck.MSTest.Tests.Arbitraries;

[TestClass]
public sealed class Arbitraries
{
    public static Arbitrary<From100To200> From100To200Arbitrary() => Arb.From(
        Gen.Choose(100, 200).Select(value => new From100To200(value)),
        from100To200 => Arb.Shrink(from100To200.Value).Where(value => value >= 100 && value <= 200).Select(value => new From100To200(value)));

    [AssemblyInitialize]
    public static void Initialize(TestContext _)
    {
        Arb.Register<Arbitraries>();
    }
}