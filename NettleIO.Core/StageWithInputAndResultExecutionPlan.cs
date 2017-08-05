using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NettleIO.Core
{
    internal class StageWithInputAndResultExecutionPlan<TStage, TStageInput, TResult> : IStageExecutionPlan
    {
        private Expression<Func<TStage, TStageInput, Task<IStageValueResult<TResult>>>> ExecutionExpression { get; }

        public StageWithInputAndResultExecutionPlan(
            Expression<Func<TStage, TStageInput, Task<IStageValueResult<TResult>>>> stageExecutionExpression)
        {
            ExecutionExpression = stageExecutionExpression ??
                                       throw new ArgumentNullException(nameof(stageExecutionExpression));
        }

        public IStagePerformer BuildPerformer()
        {
            return StagePerformerBuilder.Build(ExecutionExpression);
        }
    }
}