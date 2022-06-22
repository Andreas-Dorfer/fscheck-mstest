namespace AD.FsCheck.MSTest;

static class RunConfigurationExtensions
{
    public static IRunConfiguration OrElse(this IRunConfiguration config, IRunConfiguration? @else)
    {
        if (@else is null) return config;

        return new RunConfiguration(
            config.MaxNbOfTest > -1 ? config.MaxNbOfTest : @else.MaxNbOfTest);
    }

    public static Configuration ToConfiguration(this IRunConfiguration config, IRunner runner) => new()
    {
        MaxNbOfTest = config.MaxNbOfTest,
        Runner = runner
    };
}
