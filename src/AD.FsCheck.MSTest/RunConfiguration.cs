namespace AD.FsCheck.MSTest;

/// <summary>
/// Configures how an FsCheck test is run.
/// </summary>
/// <param name="MaxNbOfTest">The maximum number of tests that are run.</param>
/// <param name="MaxNbOfFailedTests">The maximum number of tests where values are rejected.</param>
/// <param name="Replay">If set, the seed to use to start testing.</param>
/// <param name="Verbose">Output all generated arguments.</param>
public record RunConfiguration(
    int MaxNbOfTest,
    int MaxNbOfFailedTests,
    string? Replay,
    bool Verbose) : IRunConfiguration;
