using Microsoft.FSharp.Core;

namespace AD.FsCheck.MSTest;

/// <summary>
/// Extension methods for <see cref="Config"/>.
/// </summary>
public static class ConfigurationExtensions
{
    static string? FromReplay(Replay? replay)
    {
        if (replay is null) return null;
        var replayString = $"{replay.Rnd.Seed},{replay.Rnd.Gamma}";
        var size = OptionModule.ToNullable(replay.Size);
        if (size is not null)
        {
            replayString += $",{size.Value}";
        }
        return replayString;
    }

    /// <summary>
    /// Converts a <see cref="Config"/> to a <see cref="IRunConfiguration"/>.
    /// </summary>
    /// <param name="configuration">The <see cref="Config"/> to convert.</param>
    /// <returns>The created <see cref="IRunConfiguration"/>.</returns>
    public static IRunConfiguration ToRunConfiguration(this Config configuration) =>
        new RunConfiguration(
            configuration.MaxTest,
            configuration.MaxRejected,
            configuration.StartSize,
            configuration.EndSize,
            FromReplay(OptionModule.ToObj(configuration.Replay)),
            false, //Verbose cannot be inferred from a Configuration
            configuration.QuietOnSuccess);
}
