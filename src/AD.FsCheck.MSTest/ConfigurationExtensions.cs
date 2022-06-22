namespace AD.FsCheck.MSTest;

/// <summary>
/// Extension methods for <see cref="Configuration"/>.
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Converts a <see cref="Configuration"/> to a <see cref="IRunConfiguration"/>.
    /// </summary>
    /// <param name="configuration">The <see cref="Configuration"/> to convert.</param>
    /// <returns>The created <see cref="IRunConfiguration"/>.</returns>
    public static IRunConfiguration ToRunConfiguration(this Configuration configuration) =>
        new RunConfiguration(configuration.MaxNbOfTest);
}
