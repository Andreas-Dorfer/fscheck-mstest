namespace AD.FsCheck.MSTest;

/// <summary>
/// Configures how an FsCheck test is run.
/// </summary>
/// <param name="MaxNbOfTest">The maximum number of tests that are run.</param>
public record RunConfiguration(int MaxNbOfTest) : IRunConfiguration;
