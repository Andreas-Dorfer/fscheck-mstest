namespace AD.FsCheck.MSTest;

/// <summary>
/// Configures the test run.
/// </summary>
public interface IRunConfiguration
{
    /// <summary>
    /// The maximum number of tests that are run.
    /// </summary>
    int MaxTest { get; }

    /// <summary>
    /// The maximum number of tests where values are rejected.
    /// </summary>
    int MaxRejected { get; }

    /// <summary>
    /// The size to use for the first test.
    /// </summary>
    int StartSize { get; }

    /// <summary>
    /// The size to use for the last test.
    /// </summary>
    int EndSize { get; }

    /// <summary>
    /// If set, the seed to use to start testing.
    /// </summary>
    string? Replay { get; }

    /// <summary>
    /// Output all generated arguments.
    /// </summary>
    bool Verbose { get; }

    /// <summary>
    /// Suppresses the output from the test if the test is successful.
    /// </summary>
    bool QuietOnSuccess { get; }

    /// <summary>
    /// The Arbitrary factories to use for this test method.
    /// </summary>
    Type[] ArbitraryFactory { get; }
}
