[![NuGet Package](https://img.shields.io/nuget/v/AndreasDorfer.FsCheck.MSTest.svg)](https://www.nuget.org/packages/AndreasDorfer.FsCheck.MSTest/)
# AD.FsCheck.MSTest
Integrates [FsCheck](https://fscheck.github.io/FsCheck/) with [MSTest](https://github.com/microsoft/testfx/). Inspired by FsCheck's own [Xunit integration](https://www.nuget.org/packages/FsCheck.Xunit).
## NuGet Package
    PM> Install-Package AndreasDorfer.FsCheck.MSTest -Version 0.1.0
## TLDR
Makes writing properties with MSTest easy.
```csharp
using AD.FsCheck.MSTest;

[TestClass]
public class PlusTest
{
    [Property]
    public void Plus_is_commutative(int a, int b) => Assert.AreEqual(a + b, b + a);
}
```
### Note
`AD.FsCheck.MSTest` is in an early stage.
