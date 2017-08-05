using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NettleIO.Core
{
    internal class StageWithInputExecutionPlan<TStage, TInput> : IStageExecutionPlan
    {
        private Expression<Func<TStage, TInput, Task<IStageResult>>> ExecutionExpression { get; }

        public StageWithInputExecutionPlan(Expression<Func<TStage, TInput, Task<IStageResult>>> stageExecutionExpression)
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