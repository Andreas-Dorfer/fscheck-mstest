namespace AD.FsCheck.MSTest;

/// <summary>
/// Run this method as an FsCheck test.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public partial class PropertyAttribute : TestMethodAttribute, IRunConfiguration
{
    static readonly IRunConfiguration DefaultRunConfiguration = Configuration.QuickThrowOnFailure.ToRunConfiguration();

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

    /// <summary>
    /// Returns the default <see cref="IRunConfiguration"/>.
    /// </summary>
    protected virtual IRunConfiguration Default => DefaultRunConfiguration;

    internal PropertiesAttribute? Parent { get; set; }

    /// <inheritdoc/>
    public int MaxNbOfTest { get; set; } = -1;

    public override MSTestResult[] Execute(ITestMethod testMethod)
    {
        var runConfig = this.OrElse(Parent).OrElse(Default);
        var fsCheckConfig = new Configuration
        {
            MaxNbOfTest = runConfig.MaxNbOfTest,
        };
        return Invoke(testMethod, fsCheckConfig);
    }

    MSTestResult[] Invoke(ITestMethod testMethod, Configuration fsCheckConfig)
    {
        var parameters = testMethod.ParameterTypes;
        var invokeInfo = GetInvokeMethodInfo(parameters.Length).MakeGenericMethod(parameters.Select(_ => _.ParameterType).ToArray());

        List<MSTestResult> results = new();
        ((Property)invokeInfo.Invoke(null, new object[] { (object[] values) => results.Add(testMethod.Invoke(values)) })!).Check(fsCheckConfig);
        return results.ToArray();
    }
}
