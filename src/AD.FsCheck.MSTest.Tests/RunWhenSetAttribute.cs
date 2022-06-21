namespace AD.FsCheck.MSTest.Tests;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class RunWhenSetAttribute : TestMethodAttribute
{
    readonly string environmentVariable;

    public RunWhenSetAttribute(string environmentVariable)
    {
        this.environmentVariable = environmentVariable;
    }

    public override TestResult[] Execute(ITestMethod testMethod)
    {
        if (bool.TryParse(Environment.GetEnvironmentVariable(environmentVariable), out var isSet) && isSet)
        {
            var test = testMethod.GetAttributes<TestMethodAttribute>(true).Where(_ => _ is not RunWhenSetAttribute).SingleOrDefault();
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
