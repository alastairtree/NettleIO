using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NettleIO.Core
{
    internal class StageWithResultExecutionPlan<TStage, TResult> : IStageExecutionPlan
    {
        public Expression<Func<TStage, Task<IValueResult<TResult>>>> ExecutionExpression { get; }

        public StageWithResultExecutionPlan(Expression<Func<TStage, Task<IValueResult<TResult>>>> stageExecutionExpression)
        {
            this.ExecutionExpression = stageExecutionExpression ??
                                       throw new ArgumentNullException(nameof(stageExecutionExpression));
        }

        public IStagePerformer BuildPerformer()
        {
            return StagePerformer<TStage, TResult>.Build(ExecutionExpression);
        }
    }
}