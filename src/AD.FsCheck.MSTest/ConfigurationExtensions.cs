using static FsCheck.Random;

namespace AD.FsCheck.MSTest;

/// <summary>
/// Extension methods for <see cref="Configuration"/>.
/// </summary>
public static class ConfigurationExtensions
{
    static string? FromStdGen(StdGen? stdGen)
    {
        if (stdGen is null) return null;
        return $"{stdGen.Item1},{stdGen.Item2}";
    }

    /// <summary>
    /// Converts a <see cref="Configuration"/> to a <see cref="IRunConfiguration"/>.
    /// </summary>
    /// <param name="configuration">The <see cref="Configuration"/> to convert.</param>
    /// <returns>The created <see cref="IRunConfiguration"/>.</returns>
    public static IRunConfiguration ToRunConfiguration(this Configuration configuration) =>
        new RunConfiguration(
            configuration.MaxNbOfTest,
            configuration.MaxNbOfFailedTests,
            configuration.StartSize,
            configuration.EndSize,
            FromStdGen(configuration.Replay),
            false, //Verbose cannot be inferred from a Configuration
            configuration.QuietOnSuccess);
}
