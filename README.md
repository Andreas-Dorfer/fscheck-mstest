[![NuGet Package](https://img.shields.io/nuget/v/AndreasDorfer.FsCheck.MSTest.svg)](https://www.nuget.org/packages/AndreasDorfer.FsCheck.MSTest/)
# AD.FsCheck.MSTest
Integrates [FsCheck](https://fscheck.github.io/FsCheck/) with [MSTest](https://github.com/microsoft/testfx/). Inspired by FsCheck's own [Xunit integration](https://www.nuget.org/packages/FsCheck.Xunit).
## NuGet Package
    PM> Install-Package AndreasDorfer.FsCheck.MSTest -Version 1.0.1
## TLDR
Without `AD.FsCheck.MSTest` your tests look like this:
```csharp
using FsCheck;

[TestClass]
public class PlusTest
{
    [TestMethod]
    public void Commutative() => Prop.ForAll((int a, int b) => a + b == b + a).QuickCheckThrowOnFailure();
}
```
With `AD.FsCheck.MSTest` your tests look like this:
```csharp
using AD.FsCheck.MSTest;

[TestClass]
public class PlusTest
{
    [Property]
    public void Commutative(int a, int b) => Assert.AreEqual(a + b, b + a);
}
```
## Run Configuration
You can configure how your properties are run using either the ``PropertyAttribute`` on a method, or the ``PropertiesAttribute`` on a test class:
- MaxNbOfTest
- MaxNbOfFailedTests
- StartSize
- EndSize
- Replay
- Verbose
- QuietOnSuccess
```csharp
[Properties(MaxNbOfTest = 1000)]
public class PlusTest
{
    [Property(Replay = "760375822,297103040", Verbose = true)]
    public void Commutative(int a, int b) => Assert.AreEqual(a + b, b + a);
}
```
## Lifecycle
``AD.FsCheck.MSTest`` adds two lifecycle attributes: `PropertyInitialize` and `PropertyCleanup`.
```csharp
[TestClass]
public class PlusTest
{
    [AssemblyInitialize]
    public static void AssemblyInitialize(TestContext tc)
    { /* (1) .. before any test in the assembly */ }

    [PropertyInitialize]
    public static void PropertyInitialize(string propName)
    { /* (2) .. before each property in this class */ }

    [TestInitialize]
    public void TestInitialize()
    { /* (3) .. before each individual run of a property in this class */ }

    [Property]
    public void Commutative(int a, int b)
    { /* (4) */ }

    [TestCleanup]
    public void TestCleanup()
    { /* (5) .. after each individual run of a property in this class */ }

    [PropertyCleanup]
    public static void PropertyCleanup(string propName)
    { /* (6) .. after each property in this class */ }

    [AssemblyCleanup]
    public static void AssemblyCleanup()
    { /* (7) .. after all tests in the assembly */ }
}
```
## Examples
- [C#](https://github.com/Andreas-Dorfer/fscheck-mstest/blob/09e87d3a256bbb9b7f879f233ee0782393609386/src/AD.FsCheck.MSTest.Tests/VectorTest.cs)
- [F#](https://github.com/Andreas-Dorfer/fscheck-mstest/blob/09e87d3a256bbb9b7f879f233ee0782393609386/src/AD.FsCheck.MSTest.FsTests/VectorTest.fs)
## Limitations
A property's return type **must be** `void` / `Task`.

`bool` / `Task<bool>` and `Property` / `Task<Property>` are **not supported**.
