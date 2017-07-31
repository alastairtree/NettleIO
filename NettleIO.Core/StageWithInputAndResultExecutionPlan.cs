using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NettleIO.Core
{
    //TODO: make this internal?
    internal class StageWithInputAndResultExecutionPlan<TStage, TInput, TResult> : IStageExecutionPlan
    {
        public Expression<Func<TStage, TInput, Task<IValueResult<TResult>>>> ExecutionExpression { get; }

        public StageWithInputAndResultExecutionPlan(
            Expression<Func<TStage, TInput, Task<IValueResult<TResult>>>> stageExecutionExpression)
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