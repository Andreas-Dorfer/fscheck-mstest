namespace AD.FsCheck.MSTest;

/// <summary>
/// The <see cref="PropertyCleanupAttribute"/> marks methods that are executed after every test marked
/// with a <see cref="PropertyAttribute"/>.
/// </summary>
/// <example>A cleanup method:
/// <code>
/// [PropertyCleanup]
/// public static void PropertyCleanup(string propName)
/// { }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class PropertyCleanupAttribute : Attribute
{ }
