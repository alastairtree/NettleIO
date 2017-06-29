using System;

namespace NettleIO.Core
{
    public interface IActionMetric
    {
        DateTime DateTimeUtcStarted { get; }
        DateTime DateTimeUtcEnded { get; }
        TimeSpan Duration { get; }
        int ItemsSuceeded { get; }
        int ItemsWithWarnings { get; }
        int ItemsFailed { get; }
    }
}