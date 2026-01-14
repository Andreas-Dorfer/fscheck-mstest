using System.ComponentModel;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace System.Runtime.CompilerServices;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Supports compiler features that require the IsExternalInit type for initialization-only properties. This type is not
/// intended to be used directly in application code.
/// </summary>
/// <remarks>This type is used by the C# compiler to enable the 'init' accessor for properties in versions of .NET
/// that do not natively include it. It is typically defined in user code or by libraries to provide compatibility with
/// newer language features when targeting older frameworks.</remarks>
[EditorBrowsable(EditorBrowsableState.Never)]
internal static class IsExternalInit;
