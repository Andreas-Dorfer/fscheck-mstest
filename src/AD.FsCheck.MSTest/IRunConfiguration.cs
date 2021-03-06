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
}
