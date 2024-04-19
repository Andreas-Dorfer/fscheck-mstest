namespace AD.FsCheck.MSTest.Tests.Arbitraries;

public sealed class From100To200
{
    public From100To200(int value)
    {
        if (value < 100 || value > 200) throw new ArgumentOutOfRangeException(nameof(value));
        Value = value;
    }

    public int Value { get; }
}