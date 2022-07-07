namespace AD.FsCheck.MSTest;

/// <summary>
/// The test cleanup attribute marks methods that are executed after every test marked
/// with a <see cref="PropertyInitializeAttribute"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class PropertyCleanupAttribute : Attribute
{ }
