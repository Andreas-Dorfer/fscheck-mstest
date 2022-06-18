namespace AD.FsCheck.MSTest;

/// <summary>
/// Run this method as an FsCheck test.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class PropertyAttribute : TestMethodAttribute, IConfiguration
{
    static readonly RunConfiguration DefaultRunConifguration = Configuration.QuickThrowOnFailure.ToRunConfiguration();

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

    protected virtual RunConfiguration Default => DefaultRunConifguration;

    internal PropertiesAttribute? Inherited { get; set; }

    /// <summary>
    /// The maximum number of tests that are run.
    /// </summary>
    public int MaxNbOfTest { get; set; } = -1;

    public override MSTestResult[] Execute(ITestMethod testMethod)
    {
        RunConfiguration config = new(GetEffectiveValue(_ => _.MaxNbOfTest, _ => _ > -1));

        return base.Execute(testMethod);
    }

    T GetEffectiveValue<T>(Func<IConfiguration, T> getter, Predicate<T> isSet)
    {
        var value = getter(this);
        if (isSet(value)) return value;

        if (Inherited is not null)
        {
            value = getter(Inherited);
            if (isSet(value)) return value;
        }

        return getter(Default);
    }
}
