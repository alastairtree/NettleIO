using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NettleIO.Core
{
    public static class StagePerformerBuilder
    {
        public static IStagePerformer Build<TStage>(
            Expression<Func<TStage, Task<IStageResult>>> stageExecutionExpression)
        {
            if (stageExecutionExpression == null) throw new ArgumentNullException(nameof(stageExecutionExpression));

            return new StagePerformer<TStage>(stageExecutionExpression);
        }

        public static IStagePerformer Build<TStage, TInput>(
            Expression<Func<TStage, TInput, Task<IStageResult>>> stageExecutionExpression)
        {
            if (stageExecutionExpression == null) throw new ArgumentNullException(nameof(stageExecutionExpression));

            return new StagePerformer<TStage>(stageExecutionExpression);
        }

        public static IStagePerformer Build<TStage, TResult>(
            Expression<Func<TStage, Task<IStageValueResult<TResult>>>> stageExecutionExpression)
        {
            if (stageExecutionExpression == null) throw new ArgumentNullException(nameof(stageExecutionExpression));

            return new StagePerformer<TStage, TResult>(stageExecutionExpression);
        }

        public static IStagePerformer Build<TStage,TInput,TResult>(
            Expression<Func<TStage, TInput, Task<IStageValueResult<TResult>>>> stageExecutionExpression)
        {
            if (stageExecutionExpression == null) throw new ArgumentNullException(nameof(stageExecutionExpression));

            return new StagePerformer<TStage, TResult>(stageExecutionExpression);
        }
    }
}