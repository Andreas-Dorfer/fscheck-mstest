using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;

namespace AD.FsCheck.MSTest;

class MSTestRunner : IRunner
{
    public MSTestResult? Result { get; private set; }

    public void OnArguments(int value1, FSharpList<object> value2, FSharpFunc<int, FSharpFunc<FSharpList<object>, string>> value3)
    {
        value3.Invoke(value1).Invoke(value2);
    }

    public void OnFinished(string value1, global::FsCheck.TestResult value2) =>
        Result = value2 switch
        {
            global::FsCheck.TestResult.True => new()
            {
                Outcome = MSTestOutcome.Passed,
                LogOutput = Runner.onFinishedToString("", value2)
            },
            _ => new() { Outcome = MSTestOutcome.NotRunnable }
        };

    public void OnShrink(FSharpList<object> value1, FSharpFunc<FSharpList<object>, string> value2)
    {
        value2.Invoke(value1);
    }

    public void OnStartFixture(Type value)
    { }
}
