namespace AD.FsCheck.MSTest;

/// <summary>
/// Set common configuration for all properties within this class.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class PropertiesAttribute : TestClassAttribute, IRunConfiguration
{
    /// <inheritdoc/>
    public int MaxNbOfTest { get; set; } = -1;

    public override TestMethodAttribute GetTestMethodAttribute(TestMethodAttribute testMethodAttribute)
    {
        if (testMethodAttribute is PropertyAttribute propertyAttribute)
        {
            propertyAttribute.Parent = this;
        }
        return base.GetTestMethodAttribute(testMethodAttribute);
    }
}
