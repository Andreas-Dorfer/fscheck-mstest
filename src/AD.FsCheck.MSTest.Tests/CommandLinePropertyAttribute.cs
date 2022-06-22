namespace AD.FsCheck.MSTest.Tests;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CommandLinePropertyAttribute : PropertyAttribute
{
    public CommandLinePropertyAttribute()
    { }

    public override TestResult[] Execute(ITestMethod testMethod)
    {
        var environmentVariable = Environment.GetEnvironmentVariable(CommandLineTest.EnvironmentVariable);
        if (bool.TryParse(environmentVariable, out var isSet) && isSet)
        {
            return base.Execute(testMethod);
        }
        else
        {
            return new[] { new TestResult { Outcome = UnitTestOutcome.Inconclusive } };
        }
    }
}
