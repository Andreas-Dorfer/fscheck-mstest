namespace AD.FsCheck.MSTest;

/// <summary>
/// Configures the test run.
/// </summary>
public interface IRunConfiguration
{
    /// <summary>
    /// The maximum number of tests that are run.
    /// </summary>
    int MaxNbOfTest { get; }

    /// <summary>
    /// The maximum number of tests where values are rejected.
    /// </summary>
    int MaxNbOfFailedTests { get; }

    /// <summary>
    /// If set, the seed to use to start testing.
    /// </summary>
    string? Replay { get; }

    /// <summary>
    /// Output all generated arguments.
    /// </summary>
    bool Verbose { get; }
}
