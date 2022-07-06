using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AD.FsCheck.MSTest;

/// <summary>
/// Run this method as an FsCheck test.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public partial class PropertyAttribute : TestMethodAttribute, IRunConfiguration
{
    static readonly IRunConfiguration DefaultRunConfiguration = Configuration.Quick.ToRunConfiguration();

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

    /// <inheritdoc/>
    public int MaxNbOfFailedTests { get; set; } = -1;

    /// <inheritdoc/>
    public override MSTestResult[] Execute(ITestMethod testMethod)
    {
        Action? initalizeProperty = null;
        Action? cleanupProperty = null;

        var type = testMethod.MethodInfo.DeclaringType;
        if (type is not null)
        {
            var staticMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(_ => _.ReturnType == typeof(void) && _.GetParameters().Length == 0);
            var initializeMethods = staticMethods.Where(_ => _.GetCustomAttribute<PropertyInitializeAttribute>(true) is not null);
            var cleanupMethods = staticMethods.Where(_ => _.GetCustomAttribute<PropertyCleanupAttribute>(true) is not null);

            initalizeProperty = () =>
            {
                foreach (var initialize in initializeMethods)
                {
                    initialize.Invoke(null, Array.Empty<object>());
                }
            };
            cleanupProperty = () =>
            {
                foreach (var cleanup in cleanupMethods)
                {
                    cleanup.Invoke(null, Array.Empty<object>());
                }
            };
        }

        initalizeProperty?.Invoke();

        MSTestResult[] results;

        MSTestRunner runner = new();
        var fsCheckConfig = this.OrElse(Parent).OrElse(Default).ToConfiguration(runner);
        if (TryInvoke(testMethod, fsCheckConfig, out var errorMsg))
        {
            results = new[] { runner.Result! };
        }
        else
        {
            results = new[] { new MSTestResult { Outcome = UnitTestOutcome.NotRunnable, LogError = errorMsg } };
        }

        cleanupProperty?.Invoke();

        return results;
    }

    bool TryInvoke(ITestMethod testMethod, Configuration fsCheckConfig, [NotNullWhen(false)] out string? errorMsg)
    {
        var parameters = testMethod.ParameterTypes;
        if (TryGetInvokeMethodInfo(parameters.Length, out var methodInfo, out errorMsg))
        {
            var invokeInfo = methodInfo.MakeGenericMethod(parameters.Select(_ => _.ParameterType).ToArray());
#pragma warning disable CS8974 // Converting method group to non-delegate type
            ((Property)invokeInfo.Invoke(null, new object[] { Invoke })!).Check(fsCheckConfig);
#pragma warning restore CS8974 // Converting method group to non-delegate type
            return true;
        }
        return false;

        bool Invoke(object[] values) => testMethod.Invoke(values).Outcome == UnitTestOutcome.Passed;
    }
}
