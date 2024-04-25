﻿using Microsoft.FSharp.Core;

namespace AD.FsCheck.MSTest;

static class RunConfigurationExtensions
{
    public static IRunConfiguration OrElse(this IRunConfiguration config, IRunConfiguration? @else)
    {
        if (@else is null) return config;

        return new RunConfiguration(
            config.MaxTest > -1 ? config.MaxTest : @else.MaxTest,
            config.MaxRejected > -1 ? config.MaxRejected : @else.MaxRejected,
            config.StartSize > -1 ? config.StartSize : @else.StartSize,
            config.EndSize > -1 ? config.EndSize : @else.EndSize,
            config.Replay ?? @else.Replay,
            config.Verbose || @else.Verbose,
            config.QuietOnSuccess || @else.QuietOnSuccess,
            [.. config.Arbitrary, .. @else.Arbitrary]);
    }

    static Replay? ToReplay(string? replayString)
    {
        if (replayString is null) return null;

        var items = replayString.TrimStart('(').TrimEnd(')').Split(',');
        if (items.Length < 2 || items.Length > 3) return null;

        for (int i = 0; i < items.Length; i++)
        {
            items[i] = items[i].Trim();
        }

        if (!(ulong.TryParse(items[0], out var seed) && ulong.TryParse(items[1], out var gamma))) return null;

        int? size = null;
        if (items.Length == 3)
        {
            if (!int.TryParse(items[2], out var value)) return null;
            size = value;
        }

        return new(new(seed, gamma), OptionModule.OfNullable(size));
    }

    public static Config ToConfiguration(this IRunConfiguration config, IRunner runner) => Config.Default
        .WithMaxTest(config.MaxTest)
        .WithMaxRejected(config.MaxRejected)
        .WithStartSize(config.StartSize)
        .WithEndSize(config.EndSize)
        .WithReplay(OptionModule.OfObj(ToReplay(config.Replay)))
        .WithQuietOnSuccess(config.QuietOnSuccess)
        .WithRunner(runner)
        .WithArbitrary(config.Arbitrary);
}
