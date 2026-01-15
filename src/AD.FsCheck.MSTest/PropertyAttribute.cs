using FsCheck.Fluent;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AD.FsCheck.MSTest;

/// <summary>
/// Run this method as an FsCheck test.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="PropertyAttribute"/> class.
/// </remarks>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public partial class PropertyAttribute([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1) : TestMethodAttribute(callerFilePath, callerLineNumber), IRunConfiguration
{
    static readonly IRunConfiguration DefaultRunConfiguration = Config.Quick.ToRunConfiguration();

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
    public Type[] Arbitrary { get; set; } = [];

    /// <inheritdoc/>
    public override async Task<MSTestResult[]> ExecuteAsync(ITestMethod testMethod)
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
            var (success, runException, errorMsg) = await TryInvoke(testMethod, fsCheckConfig);
            if (success)
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

    async Task<(bool Success, Exception? RunException, string? ErrorMsg)> TryInvoke(ITestMethod testMethod, Config fsCheckConfig)
    {
        var parameters = testMethod.ParameterTypes;
        if (TryGetInvokeMethodInfo(parameters.Length, out var methodInfo, out var errorMsg))
        {
            Exception? ex = default;
            async Task<bool> Invoke(object[] values)
            {
                var testResult = await testMethod.InvokeAsync(values);
                ex ??= testResult.TestFailureException;
                return testResult.Outcome == UnitTestOutcome.Passed;
            }

            var invokeInfo = methodInfo.MakeGenericMethod(parameters.Select(_ => _.ParameterType).ToArray());
            try
            {
#pragma warning disable CS8974 // Converting method group to non-delegate type
                ((Property)invokeInfo.Invoke(null, [fsCheckConfig.ArbMap, Invoke])!).Check(fsCheckConfig);
#pragma warning restore CS8974 // Converting method group to non-delegate type
                return (true, ex, errorMsg);
            }
            catch (TargetInvocationException invokeEx)
            {
                errorMsg = invokeEx.InnerException?.InnerException?.Message ?? invokeEx.InnerException?.Message ?? invokeEx.Message;
            }
        }
        return (false, RunException: default, errorMsg);
    }

    static readonly ConcurrentDictionary<Assembly, IRunConfiguration?> assemblyConfigurations = [];

    static IRunConfiguration? GetAssemblyConfiguration(Type? type) =>
        type is null ? null :
        assemblyConfigurations.GetOrAdd(type.Assembly, assembly => assembly.GetCustomAttribute<PropertiesAttribute>());
}
