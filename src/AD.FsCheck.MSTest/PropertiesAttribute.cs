namespace AD.FsCheck.MSTest;

/// <summary>
/// Set common configuration for all properties within this class.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class PropertiesAttribute : TestClassAttribute, IConfiguration
{
    /// <inheritdoc/>
    public int MaxNbOfTest { get; set; }

    public override TestMethodAttribute GetTestMethodAttribute(TestMethodAttribute testMethodAttribute)
    {
        if (testMethodAttribute is PropertyAttribute propertyAttribute)
        {
            testMethodAttribute = new PropertyAttribute(propertyAttribute.DisplayName, this);
        }
        return base.GetTestMethodAttribute(testMethodAttribute);
    }
}
