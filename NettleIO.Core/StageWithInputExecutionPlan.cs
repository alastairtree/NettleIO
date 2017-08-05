using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NettleIO.Core
{
    internal class StageWithInputExecutionPlan<TStage, TInput> : IStageExecutionPlan
    {
        public Expression<Func<TStage, TInput, Task<IActionResult>>> ExecutionExpression { get; }

        public StageWithInputExecutionPlan(Expression<Func<TStage, TInput, Task<IActionResult>>> stageExecutionExpression)
        {
            ExecutionExpression = stageExecutionExpression ??
                                       throw new ArgumentNullException(nameof(stageExecutionExpression));
        }

        public IStagePerformer BuildPerformer()
        {
            return StagePerformer<TStage>.Build(ExecutionExpression);
        }
    }
}