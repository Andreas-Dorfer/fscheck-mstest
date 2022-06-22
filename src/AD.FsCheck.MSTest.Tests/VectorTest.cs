using System.Linq;

namespace AD.FsCheck.MSTest.Tests;

public record Vector(int X, int Y)
{
    public static Vector operator +(Vector a, Vector b) => new(a.X + b.X, a.Y + b.Y);

    public static Vector PlusIdentity { get; } = new(0, 0);
}

[Properties(MaxNbOfTest = 1000)]
public class VectorTest
{
    [Property]
    public void Plus_is_commutative(Vector a, Vector b) => Assert.AreEqual(a + b, b + a);

    [Property]
    public void Plus_is_associative(Vector a, Vector b, Vector c) => Assert.AreEqual(a + b + c, a + (b + c));

    [Property]
    public void There_is_a_plus_identity_element(Vector a) => Assert.AreEqual(a, a + Vector.PlusIdentity);
}

[TestClass]
public class VectorSerializationTest
{
    [Property(MaxNbOfTest = 10)]
    public async Task Serialize_and_deserialize(Vector expected)
    {
        var data = await Serialize(expected);
        var actual = await Deserialize(data);
        Assert.AreEqual(expected, actual);
    }

    static async Task<byte[]> Serialize(Vector a)
    {
        var data = new byte[2 * sizeof(int)];
        BitConverter.GetBytes(a.X).CopyTo(data, 0);
        BitConverter.GetBytes(a.Y).CopyTo(data, sizeof(int));
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