using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NettleIO.Core
{
    internal class StageWithResultExecutionPlan<TStage, TResult> : IStageExecutionPlan
    {
        private Expression<Func<TStage, Task<IStageValueResult<TResult>>>> ExecutionExpression { get; }

        public StageWithResultExecutionPlan(Expression<Func<TStage, Task<IStageValueResult<TResult>>>> stageExecutionExpression)
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