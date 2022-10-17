using FsCheck;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace AD.FsCheck.MSTest.Tests;

public abstract class CommandLineTest : IDisposable
{
    public enum Fetch
    {
        StdOut,
        StdErr,
        Message
    }

    public const string EnvironmentVariable = "CommandLine";

    readonly string className;
    readonly string fileName;

    public CommandLineTest(string className)
    {
        this.className = className;
        fileName = CreateTestFileName();
    }

    protected async Task<int> AssertSuccess(string testName)
    {
        var result = await Run(testName, Fetch.StdOut);
        var match = Regex.Match(result, @"^Ok, passed (\d+) tests.$");
        IsTrue(match.Success);
        return int.Parse(match.Groups[1].Value);
    }

    static void AssertIsFalsifiableText(string text) => IsTrue(text.StartsWith("Falsifiable"));

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
        IsTrue(message.EndsWith(expectedMessage));
    }

    protected async Task<string> Run(string testName, Fetch fetch)
    {
        await RunCommandLineTest(testName);
        var output = await FetchTestOutput(fetch);
        return output;
    }

    static string CreateTestFileName() => Path.ChangeExtension(Path.GetTempFileName(), ".trx");

    async Task RunCommandLineTest(string testName) =>
        await Process.Start("dotnet.exe", $@"test ..\..\..\. --no-build --environment {EnvironmentVariable}=true --logger ""trx;LogFileName={fileName}"" --filter ""FullyQualifiedName=AD.FsCheck.MSTest.Tests.{className}.{testName}""").WaitForExitAsync();

    async Task<string> FetchTestOutput(Fetch fetch)
    {
        using var reader = new StreamReader(fileName);
        var result = await XDocument.LoadAsync(reader, LoadOptions.None, CancellationToken.None);
        return result.Descendants(XName.Get(GetFetchName(fetch), "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")).Single().Value;
    }

    static string GetFetchName(Fetch fetch) =>
        fetch switch
        {
            Fetch.StdErr => "StdErr",
            Fetch.Message => "Message",
            _ => "StdOut"
        };

    public void Dispose()
    {
        if (fileName is not null)
        {
            File.Delete(fileName);
        }
    }
}
