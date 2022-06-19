namespace AD.FsCheck.MSTest;

internal static class ConfigurationExtensions
{
    public static IRunConfiguration ToRunConfiguration(this Configuration configuration) =>
        new RunConfiguration(configuration.MaxNbOfTest);
}
