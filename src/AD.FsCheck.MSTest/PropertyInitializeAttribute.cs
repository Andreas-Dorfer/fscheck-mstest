namespace AD.FsCheck.MSTest;

/// <summary>
/// The <see cref="PropertyInitializeAttribute"/> marks methods that are executed before every test marked
/// with a <see cref="PropertyAttribute"/>.
/// </summary>
/// <example>An initialization method:
/// <code>
/// [PropertyInitialize]
/// public static void PropertyInitialize(string propName)
/// { }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class PropertyInitializeAttribute : Attribute
{ }
