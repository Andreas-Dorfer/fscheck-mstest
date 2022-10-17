﻿namespace AD.FsCheck.MSTest.Tests;

[TestClass]
public class PropertyCleanupExceptionTest : CommandLineTest
{
    public PropertyCleanupExceptionTest() : base(nameof(PropertyCleanupExceptionTest))
    { }

    [PropertyCleanup]
    public static void PropertyCleanup() => Fail(nameof(PropertyCleanup));

    [CommandLineProperty]
    public void Prop(int _)
    { }

    [TestMethod]
    public async Task Prop_test() => await AssertMessage(nameof(Prop), nameof(PropertyCleanup));
}
