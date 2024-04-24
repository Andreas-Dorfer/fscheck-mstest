using FsCheck.Fluent;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AD.FsCheck.MSTest;

/// <summary>
/// Run this method as an FsCheck test.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public partial class PropertyAttribute : TestMethodAttribute, IRunConfiguration
{
    static readonly IRunConfiguration DefaultRunConfiguration = Config.Quick.ToRunConfiguration();

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
    public int MaxTest { get; set; } = -1;

    /// <inheritdoc/>
    public int MaxRejected { get; set; } = -1;

    /// <inheritdoc/>
    public int StartSize { get; set; } = -1;

    /// <inheritdoc/>
    public int EndSize { get; set; } = -1;

    /// <inheritdoc/>
    public string? Replay { get; set; }

    /// <inheritdoc/>
    public bool Verbose { get; set; }

    /// <inheritdoc/>
    public bool QuietOnSuccess { get; set; }

    /// <inheritdoc/>
    public Type[] ArbitraryFactory { get; set; } = [];

    /// <inheritdoc/>
    public override MSTestResult[] Execute(ITestMethod testMethod)
    {
        var stopWatch = Stopwatch.StartNew();

        var propName = testMethod.TestMethodName;

        Action? initalizeProperty = null;
        Action? cleanupProperty = null;

        var type = testMethod.MethodInfo.DeclaringType;
        if (type is not null)
        {
            var staticMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(method =>
            {
                if (method.ReturnType != typeof(void)) return false;
                var parameters = method.GetParameters();
                return parameters.Length == 1 && parameters[0].ParameterType == typeof(string);
            });
            var initializeMethods = staticMethods.Where(_ => _.GetCustomAttribute<PropertyInitializeAttribute>(true) is not null);
            var cleanupMethods = staticMethods.Where(_ => _.GetCustomAttribute<PropertyCleanupAttribute>(true) is not null);

            initalizeProperty = () =>
            {
                foreach (var initialize in initializeMethods)
                {
                    initialize.Invoke(null, new[] { propName });
                }
            };
            cleanupProperty = () =>
            {
                foreach (var cleanup in cleanupMethods)
                {
                    cleanup.Invoke(null, new[] { propName });
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
            var assemblyConfig = GetAssemblyConfiguration(testMethod.MethodInfo.DeclaringType);
            var combined = this.OrElse(Parent).OrElse(assemblyConfig).OrElse(Default);
            MSTestRunner runner = new(combined.Verbose, combined.QuietOnSuccess);
            var fsCheckConfig = combined.ToConfiguration(runner);
            var arbMap = ArbMap.Default;
            if (TryInvoke(testMethod, fsCheckConfig, BuildArbMap(combined), out var runException, out var errorMsg))
            {
                var runResult = runner.Result!;
                runResult.TestFailureException = runException;
                results = [runResult];
            }
            else
            {
                results = [new MSTestResult { Outcome = UnitTestOutcome.NotRunnable, LogError = errorMsg }];
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

        results ??= [new MSTestResult { Outcome = UnitTestOutcome.Failed, TestFailureException = initializeCleanupException }];
        stopWatch.Stop();
        results[0].Duration = stopWatch.Elapsed;
        return results;
    }

    bool TryInvoke(ITestMethod testMethod, Config fsCheckConfig, IArbMap arbMap, out Exception? runException, [NotNullWhen(false)] out string? errorMsg)
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
            try
            {
#pragma warning disable CS8974 // Converting method group to non-delegate type
                ((Property)invokeInfo.Invoke(null, [arbMap, Invoke])!).Check(fsCheckConfig);
#pragma warning restore CS8974 // Converting method group to non-delegate type
                runException = ex;
                return true;
            }
            catch (TargetInvocationException invokeEx)
            {
                errorMsg = invokeEx.InnerException?.Message ?? invokeEx.Message;
            }
        }
        runException = default;
        return false;
    }

    static readonly ConcurrentDictionary<Assembly, IRunConfiguration?> assemblyConfigurations = [];

    static IRunConfiguration? GetAssemblyConfiguration(Type? type) =>
        type is null ? null :
        assemblyConfigurations.GetOrAdd(type.Assembly, assembly => assembly.GetCustomAttribute<PropertiesAttribute>());

    static IArbMap BuildArbMap(IRunConfiguration runConfiguration)
    {
        var arbMap = ArbMap.Default;

        foreach (var factoryType in runConfiguration.ArbitraryFactory)
        {
            var merge = factoryType.GetMethod("Merge", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod);
            if (merge is null ||
                merge.IsGenericMethod ||
                merge.ReturnType != typeof(IArbMap)) continue;
            var parameter = merge.GetParameters();
            if (parameter.Length != 1 ||
                parameter[0].ParameterType != typeof(IArbMap)) continue;

            var next = (IArbMap)merge.Invoke(null, [arbMap])!;
            if (next is null) continue;
            arbMap = next;
        }

        return arbMap;
    }
}
