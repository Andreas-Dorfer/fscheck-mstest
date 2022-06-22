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
    public void Commutative(int a, int b) => Assert.AreEqual(a + b, b + a);
}
```
## Examples
- [C#](https://github.com/Andreas-Dorfer/fscheck-mstest/blob/09e87d3a256bbb9b7f879f233ee0782393609386/src/AD.FsCheck.MSTest.Tests/VectorTest.cs)
- [F#](https://github.com/Andreas-Dorfer/fscheck-mstest/blob/09e87d3a256bbb9b7f879f233ee0782393609386/src/AD.FsCheck.MSTest.FsTests/VectorTest.fs)
## Limitations
A property's return type **must be** `void` / `Task`.

`bool` / `Task<bool>` and `Property` / `Task<Property>` are **not supported**.
### Note
`AD.FsCheck.MSTest` is in an early stage.
