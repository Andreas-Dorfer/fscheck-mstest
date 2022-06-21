namespace AD.FsCheck.MSTest.Tests;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CommandLineAttribute : TestMethodAttribute
{
    readonly bool commandLine;

    public CommandLineAttribute(bool commandLine)
    {
        this.commandLine = commandLine;
    }

    public override TestResult[] Execute(ITestMethod testMethod)
    {
        var environmentVariable = Environment.GetEnvironmentVariable(CommandLineTest.EnvironmentVariable);
        if (commandLine ?
            bool.TryParse(environmentVariable, out var isSet) && isSet :
            !bool.TryParse(environmentVariable, out isSet) || !isSet)
        {
            var test = testMethod.GetAttributes<TestMethodAttribute>(true).Where(_ => _ is not CommandLineAttribute).SingleOrDefault();
            if (test is not null)
            {
                return test.Execute(testMethod);
            }
            return base.Execute(testMethod);
        }
        else
        {
            return new[] { new TestResult { Outcome = UnitTestOutcome.Inconclusive } };
        }
    }
}
