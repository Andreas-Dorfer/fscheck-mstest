namespace AD.FsCheck.MSTest.Tests;

[TestClass]
public sealed class CleanupTest
{
    static int count = 2;

    [PropertyCleanup]
    public static void PropertyCleanup(string propName)
    {
        CollectionAssert.Contains(new[] { nameof(PropA), nameof(PropB) }, propName);
        count--;
    }

    [ClassCleanup]
    public static void ClassClenup()
    {
        AreEqual(0, count);
    }

    [Property]
    public void PropA(int a) => AreEqual(a, a);

    [Property]
    public void PropB(int a) => AreEqual(a, a);
}
