namespace AD.FsCheck.MSTest;

internal interface IConfiguration
{
    /// <summary>
    /// The maximum number of tests that are run.
    /// </summary>
    int MaxNbOfTest { get; set; }
}
