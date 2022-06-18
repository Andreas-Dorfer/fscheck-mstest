namespace AD.FsCheck.MSTest;

/// <summary>
/// Set common configuration for all properties within this class.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class PropertiesAttribute : TestClassAttribute, IConfiguration
{
    /// <summary>
    /// The maximum number of tests that are run.
    /// </summary>
    public int MaxNbOfTest { get; set; } = -1;

    public override TestMethodAttribute GetTestMethodAttribute(TestMethodAttribute testMethodAttribute)
    {
        if (testMethodAttribute is PropertyAttribute propertyAttribute)
        {
            propertyAttribute.Inherited = this;
        }
        return base.GetTestMethodAttribute(testMethodAttribute);
    }
}
