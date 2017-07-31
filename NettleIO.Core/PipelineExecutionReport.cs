using System.Collections.Generic;
using System.Linq;

namespace NettleIO.Core
{
    public class PipelineExecutionReport
    {
        private readonly List<IActionResult> results;

        public PipelineExecutionReport(List<IActionResult> results)
        {
            this.results = results;
        }

        public bool Succeeded { get { return results.Any() && results.All(x => x.Succeeded); } }
    }
}