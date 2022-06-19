using System.Reflection;
using System.Runtime.CompilerServices;

namespace AD.FsCheck.MSTest;

/// <summary>
/// Run this method as an FsCheck test.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class PropertyAttribute : TestMethodAttribute, IRunConfiguration
{
    static readonly IRunConfiguration DefaultRunConfiguration = Configuration.QuickThrowOnFailure.ToRunConfiguration();
    static readonly MethodInfo Invoke1Info = typeof(PropertyAttribute).GetMethod(nameof(Invoke1), BindingFlags.Static | BindingFlags.NonPublic)!;
    static readonly MethodInfo Invoke2Info = typeof(PropertyAttribute).GetMethod(nameof(Invoke2), BindingFlags.Static | BindingFlags.NonPublic)!;
    static readonly MethodInfo Invoke3Info = typeof(PropertyAttribute).GetMethod(nameof(Invoke3), BindingFlags.Static | BindingFlags.NonPublic)!;
    static readonly MethodInfo Invoke4Info = typeof(PropertyAttribute).GetMethod(nameof(Invoke4), BindingFlags.Static | BindingFlags.NonPublic)!;

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

    static MSTestResult[] Invoke(ITestMethod testMethod, Configuration fsCheckConfig)
    {
        var parameters = testMethod.ParameterTypes;
        var genericInvokeInfo = parameters.Length switch
        {
            0 => throw new InvalidOperationException($"Properties must have at least one parameter."),
            1 => Invoke1Info,
            2 => Invoke2Info,
            3 => Invoke3Info,
            4 => Invoke4Info,
            _ => throw new InvalidOperationException($"The number of property parameters is limited to four. The actual number is {parameters.Length}.")
        };
        var invokeInfo = genericInvokeInfo.MakeGenericMethod(parameters.Select(_ => _.ParameterType).ToArray());

        List<MSTestResult> results = new();
        var prop = (Property)invokeInfo.Invoke(null, new object[] { (object[] values) => results.Add(testMethod.Invoke(values)) })!;
        prop.Check(fsCheckConfig);
        return results.ToArray();
    }

    static Property Invoke1<T>(Action<object[]> method) =>
        Prop.ForAll<T>(v => method(new object[] { v! }));

    static Property Invoke2<T1, T2>(Action<object[]> method) =>
        Prop.ForAll<T1, T2>((v1, v2) => method(new object[] { v1!, v2! }));

    static Property Invoke3<T1, T2, T3>(Action<object[]> method) =>
        Prop.ForAll<T1, T2, T3>((v1, v2, v3) => method(new object[] { v1!, v2!, v3! }));

    static Property Invoke4<T1, T2, T3, T4>(Action<object[]> method) =>
        Prop.ForAll<(T1, T2), T3, T4>((v1_2, v3, v4) => method(new object[] { v1_2.Item1!, v1_2.Item2!, v3!, v4! }));
}
