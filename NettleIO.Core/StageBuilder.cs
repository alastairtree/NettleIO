using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NettleIO.Core
{
    public class StageBuilder<TStage>
    {
        private readonly PipelineBuilder pipelineBuilder;

        internal StageBuilder(PipelineBuilder pipelineBuilder)
        {
            this.pipelineBuilder = pipelineBuilder ?? throw new ArgumentNullException(nameof(pipelineBuilder));
        }

        public StageWithInputCreator<TResult> AndDo<TResult>(
            Expression<Func<TStage, Task<IStageValueResult<TResult>>>> stageExecutionExpression)
        {
            if (stageExecutionExpression == null) throw new ArgumentNullException(nameof(stageExecutionExpression));

            var plan = new StageWithResultExecutionPlan<TStage, TResult>(stageExecutionExpression);
            pipelineBuilder.AddSourcePlan(plan);

            return new StageWithInputCreator<TResult>(pipelineBuilder);
            ;
        }
    }

    public class StageBuilder<TStage, TInput>
    {
        private readonly PipelineBuilder pipelineBuilder;

        public StageBuilder(PipelineBuilder pipelineBuilder)
        {
            this.pipelineBuilder = pipelineBuilder ?? throw new ArgumentNullException(nameof(pipelineBuilder));
        }

        public StageWithInputCreator<TResult> AndDo<TResult>(
            Expression<Func<TStage, TInput, Task<IStageValueResult<TResult>>>> stageExecutionExpression)
        {
            if (stageExecutionExpression == null) throw new ArgumentNullException(nameof(stageExecutionExpression));
            var plan = new StageWithInputAndResultExecutionPlan<TStage, TInput, TResult>(stageExecutionExpression);
            pipelineBuilder.AddStagePlan(plan);
            return new StageWithInputCreator<TResult>(pipelineBuilder);
        }

        public PipelineBuilder AndDo(Expression<Func<TStage, TInput, Task<IStageResult>>> stageExecutionExpression)
        {
            if (stageExecutionExpression == null) throw new ArgumentNullException(nameof(stageExecutionExpression));
            var plan = new StageWithInputExecutionPlan<TStage, TInput>(stageExecutionExpression);
            pipelineBuilder.AddStagePlan(plan);
            return pipelineBuilder;
        }
    }
}