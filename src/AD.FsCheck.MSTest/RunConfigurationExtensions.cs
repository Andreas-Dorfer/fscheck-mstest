using static FsCheck.Random;

namespace AD.FsCheck.MSTest;

static class RunConfigurationExtensions
{
    public static IRunConfiguration OrElse(this IRunConfiguration config, IRunConfiguration? @else)
    {
        if (@else is null) return config;

        return new RunConfiguration(
            config.MaxNbOfTest > -1 ? config.MaxNbOfTest : @else.MaxNbOfTest,
            config.MaxNbOfFailedTests > 1 ? config.MaxNbOfFailedTests : @else.MaxNbOfFailedTests,
            config.Replay ?? @else.Replay,
            config.Verbose || @else.Verbose);
    }

    static StdGen? ToStdGen(string? replay)
    {
        if (replay is null) return null;

        var items = replay.TrimStart('(').TrimEnd(')').Split(',');
        if (items.Length != 2) return null;

        for (int i = 0; i < items.Length; i++)
        {
            items[i] = items[i].Trim();
        }

        if (!(int.TryParse(items[0], out var item1) && int.TryParse(items[1], out var item2))) return null;
        return StdGen.NewStdGen(item1, item2);
    }

    public static Configuration ToConfiguration(this IRunConfiguration config, IRunner runner) => new()
    {
        MaxNbOfTest = config.MaxNbOfTest,
        MaxNbOfFailedTests = config.MaxNbOfFailedTests,
        Replay = ToStdGen(config.Replay),
        Runner = runner
    };
}
