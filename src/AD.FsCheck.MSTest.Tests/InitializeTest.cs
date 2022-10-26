﻿namespace AD.FsCheck.MSTest.Tests;

[TestClass]
public class InitializeTest : IDisposable
{
    static int count;

    [PropertyInitialize]
    public static void PropertyInitialize(string propName)
    {
        CollectionAssert.Contains(new[] { nameof(PropA), nameof(PropB) }, propName);
        count++;
    }

    [ClassCleanup(ClassCleanupBehavior.EndOfClass)]
    public static void ClassClenup()
    {
        AreEqual(2, count);
    }

    [Property]
    public void PropA(int a) => AreEqual(a, a);

    [Property]
    public void PropB(int a) => AreEqual(a, a);

    public void Dispose()
    {
        IsTrue(count > 0);
    }
}
