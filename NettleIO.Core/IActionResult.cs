using System;

namespace NettleIO.Core
{
    public interface IActionResult
    {
        bool Succeeded { get; }

        Exception Error { get; }

        //TODO: Work out if this should live here or on some context object?
        IActionMetric Metrics { get; }

        string Message { get; }
    }
}