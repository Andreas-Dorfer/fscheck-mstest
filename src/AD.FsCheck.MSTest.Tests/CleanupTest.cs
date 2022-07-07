namespace AD.FsCheck.MSTest.Tests;

[TestClass]
public class CleanupTest
{
    static int count = 2;

    [PropertyCleanup]
    public static void PropertyCleanup()
    {
        count--;
    }

    [ClassCleanup(ClassCleanupBehavior.EndOfClass)]
    public static void ClassClenup()
    {
        AreEqual(0, count);
    }

    [Property]
    public void PropA(int a) => AreEqual(a, a);

    [Property]
    public void PropB(int a) => AreEqual(a, a);
}
