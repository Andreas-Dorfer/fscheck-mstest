namespace AD.FsCheck.MSTest;

internal static class ConfigurationExtensions
{
    public static RunConfiguration ToRunConfiguration(this Configuration configuration) =>
        new(configuration.MaxNbOfTest);
}
