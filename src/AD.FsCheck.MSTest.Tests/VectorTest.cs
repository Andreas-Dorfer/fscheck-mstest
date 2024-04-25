namespace AD.FsCheck.MSTest.Tests;

public record Vector(int X, int Y)
{
    public static Vector operator +(Vector a, Vector b) => new(a.X + b.X, a.Y + b.Y);

    public static Vector PlusIdentity { get; } = new(0, 0);
}

[Properties(MaxTest = 1000)]
public sealed class VectorTest
{
    [Property]
    public void Plus_is_commutative(Vector a, Vector b) => AreEqual(a + b, b + a);

    [Property]
    public void Plus_is_associative(Vector a, Vector b, Vector c) => AreEqual(a + b + c, a + (b + c));

    [Property]
    public void There_is_a_plus_identity_element(Vector a) => AreEqual(a, a + Vector.PlusIdentity);
}

[TestClass]
public sealed class VectorSerializationTest
{
    [Property(MaxTest = 10)]
    public async Task Serialize_and_deserialize(Vector expected)
    {
        var actual = await Deserialize(await Serialize(expected));
        AreEqual(expected, actual);
    }

    static async Task<byte[]> Serialize(Vector vector)
    {
        var data = new byte[2 * sizeof(int)];
        BitConverter.GetBytes(vector.X).CopyTo(data, 0);
        BitConverter.GetBytes(vector.Y).CopyTo(data, sizeof(int));
        await Task.Delay(10);
        return data;
    }

    static async Task<Vector> Deserialize(byte[] data)
    {
        Vector vector = new(BitConverter.ToInt32(data, 0), BitConverter.ToInt32(data, sizeof(int)));
        await Task.Delay(10);
        return vector;
    }
}