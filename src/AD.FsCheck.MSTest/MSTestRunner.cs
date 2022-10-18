using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;
using System.Text;

namespace AD.FsCheck.MSTest;

class MSTestRunner : IRunner
{
    readonly bool verbose, quietOnSuccess;
    readonly StringBuilder log = new();

    public MSTestRunner(bool verbose, bool quietOnSuccess)
    {
        this.verbose = verbose;
        this.quietOnSuccess = quietOnSuccess;
    }

    public MSTestResult? Result { get; private set; }

    public void OnStartFixture(Type t)
    { }

    public void OnArguments(int ntest, FSharpList<object> args, FSharpFunc<int, FSharpFunc<FSharpList<object>, string>> every)
    {
        if (verbose)
        {
            log.AppendLine($"{ntest}: ({string.Join(", ", args)})");
        }
        every.Invoke(ntest).Invoke(args);
    }

    public void OnShrink(FSharpList<object> args, FSharpFunc<FSharpList<object>, string> everyShrink)
    {
        if(verbose)
        {
            log.AppendLine($"shrink: ({string.Join(", ", args)})");
        }
        everyShrink.Invoke(args);
    }

    public void OnFinished(string name, FsCheckResult testResult) =>
        Result = testResult switch
        {
            FsCheckResult.True => new()
            {
                Outcome = MSTestOutcome.Passed,
                LogOutput = quietOnSuccess ? null : log.Append(Runner.onFinishedToString("", testResult)).ToString()
            },
            FsCheckResult.False => new()
            {
                Outcome = MSTestOutcome.Failed,
                LogError = log.Append(Runner.onFinishedToString("", testResult)).ToString()
            },
            _ => new() { Outcome = MSTestOutcome.NotRunnable }
        };
}
