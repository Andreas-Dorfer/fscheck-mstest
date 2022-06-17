namespace AD.FsCheck.MSTest;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class PropertiesAttribute : TestClassAttribute
{
    public override TestMethodAttribute GetTestMethodAttribute(TestMethodAttribute testMethodAttribute)
    {
        return base.GetTestMethodAttribute(testMethodAttribute);
    }
}
