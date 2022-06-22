using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;

namespace AD.FsCheck.MSTest;

class MSTestRunner : IRunner
{
    public MSTestResult? Result { get; private set; }

    public void OnStartFixture(Type t)
    { }

    public void OnArguments(int ntest, FSharpList<object> args, FSharpFunc<int, FSharpFunc<FSharpList<object>, string>> every) =>
        every.Invoke(ntest).Invoke(args);

    public void OnShrink(FSharpList<object> args, FSharpFunc<FSharpList<object>, string> everyShrink) =>
        everyShrink.Invoke(args);

    public void OnFinished(string name, FsCheckResult testResult) =>
        Result = testResult switch
        {
            FsCheckResult.True => new()
            {
                Outcome = MSTestOutcome.Passed,
                LogOutput = Runner.onFinishedToString("", testResult)
            },
            FsCheckResult.False => new()
            {
                Outcome = MSTestOutcome.Failed,
                LogError = Runner.onFinishedToString("", testResult)
            },
            _ => new() { Outcome = MSTestOutcome.NotRunnable }
        };
}
