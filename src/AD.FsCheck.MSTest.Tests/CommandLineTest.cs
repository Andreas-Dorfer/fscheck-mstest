using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace AD.FsCheck.MSTest.Tests;

public abstract partial class CommandLineTest(string className) : IDisposable
{
    public enum Fetch
    {
        StdOut,
        StdErr,
        Message,
        Duration
    }

    public const string EnvironmentVariable = "CommandLine";

    readonly string fileName = Path.ChangeExtension(Path.GetTempFileName(), ".trx");

    protected async Task<int> AssertSuccess(string testName)
    {
        var result = await Run(testName, Fetch.StdOut);
        var match = OkRegex().Match(result);
        IsTrue(match.Success);
        return int.Parse(match.Groups[1].Value);
    }

    static void AssertIsFalsifiableText(string text) => StartsWith("Falsifiable", text);

    protected async Task AssertFalsifiable(string testName)
    {
        var result = await Run(testName, Fetch.StdErr);
        AssertIsFalsifiableText(result);
    }

    protected async Task AssertFalsifiableWithMessage(string testName, string expectedMessage)
    {
        await AssertFalsifiable(testName);
        var result = await FetchTestOutput(Fetch.StdErr);
        var message = await FetchTestOutput(Fetch.Message);

        AssertIsFalsifiableText(result);
        EndsWith(expectedMessage, message);
    }

    protected async Task AssertMessage(string testName, string expectedMessage)
    {
        var message = await Run(testName, Fetch.Message);
        EndsWith(expectedMessage, message);
    }

    protected async Task<string> Run(string testName, Fetch fetch)
    {
        await RunCommandLineTest(testName);
        var output = await FetchTestOutput(fetch);
        return output;
    }

    async Task RunCommandLineTest(string testName) =>
        await Process.Start("dotnet", @"test ..\..\..\." +
#if RELEASE
            " --configuration Release" +
#endif
            $@" --no-build --environment {EnvironmentVariable}=true --logger ""trx;LogFileName={fileName}"" --filter ""FullyQualifiedName=AD.FsCheck.MSTest.Tests.{className}.{testName}""").WaitForExitAsync();

    async Task<string> FetchTestOutput(Fetch fetch)
    {
        using var reader = new StreamReader(fileName);
        var result = await XDocument.LoadAsync(reader, LoadOptions.None, CancellationToken.None);
        if (fetch == Fetch.Duration)
        {
            return result.Descendants(XName.Get("UnitTestResult", "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")).Single().Attribute("duration")!.Value;
        }
        else
        {
            return result.Descendants(XName.Get(GetFetchName(fetch), "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")).Single().Value;
        }
    }

    static string GetFetchName(Fetch fetch) =>
        fetch switch
        {
            Fetch.StdErr => "StdErr",
            Fetch.Message => "Message",
            _ => "StdOut"
        };

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
    public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    {
        if (fileName is not null)
        {
            File.Delete(fileName);
        }
    }

    [GeneratedRegex("^Ok, passed (\\d+) tests.$")]
    private static partial Regex OkRegex();
}
