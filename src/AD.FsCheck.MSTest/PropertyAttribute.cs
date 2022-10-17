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

        Exception? initializeCleanupException = default;
        try
        {
            initalizeProperty?.Invoke();
        }
        catch (TargetInvocationException ex)
        {
            initializeCleanupException = ex.InnerException;
        }

        MSTestResult[]? results = default;

        if (initializeCleanupException is null)
        {
            MSTestRunner runner = new();
            var fsCheckConfig = this.OrElse(Parent).OrElse(Default).ToConfiguration(runner);
            if (TryInvoke(testMethod, fsCheckConfig, out var runException, out var errorMsg))
            {
                var runResult = runner.Result!;
                runResult.TestFailureException = runException;
                results = new[] { runResult };
            }
            else
            {
                results = new[] { new MSTestResult { Outcome = UnitTestOutcome.NotRunnable, LogError = errorMsg } };
            }
        }

        try
        {
            cleanupProperty?.Invoke();
        }
        catch (TargetInvocationException ex)
        {
            results = default;
            initializeCleanupException ??= ex.InnerException;
        }

        return results ?? new[] { new MSTestResult { Outcome = UnitTestOutcome.Failed, TestFailureException = initializeCleanupException } };
    }

    bool TryInvoke(ITestMethod testMethod, Configuration fsCheckConfig, out Exception? runException, [NotNullWhen(false)] out string? errorMsg)
    {
        var parameters = testMethod.ParameterTypes;
        if (TryGetInvokeMethodInfo(parameters.Length, out var methodInfo, out errorMsg))
        {
            Exception? ex = default;
            bool Invoke(object[] values)
            {
                var testResult = testMethod.Invoke(values);
                ex ??= testResult.TestFailureException;
                return testResult.Outcome == UnitTestOutcome.Passed;
            }

            var invokeInfo = methodInfo.MakeGenericMethod(parameters.Select(_ => _.ParameterType).ToArray());
#pragma warning disable CS8974 // Converting method group to non-delegate type
            ((Property)invokeInfo.Invoke(null, new object[] { Invoke })!).Check(fsCheckConfig);
#pragma warning restore CS8974 // Converting method group to non-delegate type
            runException = ex;
            return true;
        }
        runException = default;
        return false;
    }
}
