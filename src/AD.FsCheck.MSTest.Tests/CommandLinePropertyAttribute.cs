
using System.Runtime.CompilerServices;

namespace AD.FsCheck.MSTest.Tests;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class CommandLinePropertyAttribute([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1) : PropertyAttribute(callerFilePath, callerLineNumber)
{
    public override async Task<TestResult[]> ExecuteAsync(ITestMethod testMethod)
    {
        var environmentVariable = Environment.GetEnvironmentVariable(CommandLineTest.EnvironmentVariable);
        if (bool.TryParse(environmentVariable, out var isSet) && isSet)
        {
            return await base.ExecuteAsync(testMethod);
        }
        else
        {
            return [new TestResult { Outcome = UnitTestOutcome.Inconclusive }];
        }
    }
}
