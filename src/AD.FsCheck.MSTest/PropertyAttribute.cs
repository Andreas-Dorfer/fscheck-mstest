﻿using System.Diagnostics.CodeAnalysis;

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
        MSTestRunner runner = new();
        var fsCheckConfig = this.OrElse(Parent).OrElse(Default).ToConfiguration(runner);
        if (TryInvoke(testMethod, fsCheckConfig, out var errorMsg))
        {
            return new[] { runner.Result! };
        }
        return new[] { new MSTestResult { Outcome = UnitTestOutcome.NotRunnable, LogError = errorMsg } };
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
