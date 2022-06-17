namespace AD.FsCheck.MSTest;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class PropertyAttribute : TestMethodAttribute
{
    public PropertyAttribute()
    { }

    public PropertyAttribute(string displayName) : base(displayName)
    { }

    public override TestResult[] Execute(ITestMethod testMethod)
    {
        return base.Execute(testMethod);
    }
}
