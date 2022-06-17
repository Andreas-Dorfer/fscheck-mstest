namespace AD.FsCheck.MSTest;

/// <summary>
/// Run this method as an FsCheck test.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class PropertyAttribute : TestMethodAttribute, IConfiguration
{
    readonly IConfiguration? inheritedConfiguration;

    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyAttribute"/> class.
    /// </summary>
    public PropertyAttribute()
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyAttribute"/> class.
    /// </summary>
    /// <param name="displayName"><inheritdoc/></param>
    public PropertyAttribute(string displayName) : base(displayName)
    { }

    internal PropertyAttribute(string displayName, IConfiguration inheritedConfiguration) : this(displayName)
    {
        this.inheritedConfiguration = inheritedConfiguration;
    }

    /// <inheritdoc/>
    public int MaxNbOfTest { get; set; }

    public override TestResult[] Execute(ITestMethod testMethod)
    {
        return base.Execute(testMethod);
    }
}
