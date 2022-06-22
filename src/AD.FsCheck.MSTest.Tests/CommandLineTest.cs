using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace AD.FsCheck.MSTest.Tests;

public abstract class CommandLineTest
{
    public enum Fetch
    {
        StdOut,
        StdErr
    }

    public const string EnvironmentVariable = "CommandLine";

    readonly string className;

    public CommandLineTest(string className)
    {
        this.className = className;
    }

    protected async Task<int> AssertSuccess(string testName)
    {
        var result = await Run(testName, Fetch.StdOut);
        var match = Regex.Match(result, @"^Ok, passed (\d+) tests.$");
        IsTrue(match.Success);
        return int.Parse(match.Groups[1].Value);
    }

    protected async Task AssertFalsifiable(string testName)
    {
        var result = await Run(testName, Fetch.StdErr);
        IsTrue(result.StartsWith("Falsifiable"));
    }

    protected async Task<string> Run(string testName, Fetch fetch)
    {
        var fileName = CreateTestFileName();
        await RunCommandLineTest(testName, fileName);
        var output = await FetchTestOutput(fileName, fetch);
        File.Delete(fileName);
        return output;
    }

    static string CreateTestFileName() => Path.ChangeExtension(Path.GetTempFileName(), ".trx");

    async Task RunCommandLineTest(string testName, string fileName) =>
        await Process.Start("dotnet.exe", $@"test ..\..\..\. --no-build --environment {EnvironmentVariable}=true --logger ""trx;LogFileName={fileName}"" --filter ""FullyQualifiedName=AD.FsCheck.MSTest.Tests.{className}.{testName}""").WaitForExitAsync();

    static async Task<string> FetchTestOutput(string fileName, Fetch fetch)
    {
        using var reader = new StreamReader(fileName);
        var result = await XDocument.LoadAsync(reader, LoadOptions.None, CancellationToken.None);
        return result.Descendants(XName.Get(GetFetchName(fetch), "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")).Single().Value;
    }

    static string GetFetchName(Fetch fetch) =>
        fetch switch
        {
            Fetch.StdErr => "StdErr",
            _ => "StdOut"
        };
}
