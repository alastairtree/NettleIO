using System;
using System.Collections.Generic;

namespace NettleIO.Core
{
    public class PipelineBuilder
    {
        private readonly List<IStageExecutionPlan> stagePlans = new List<IStageExecutionPlan>();

        public static StageBuilder<T> StartWithSource<T>()
        {
            return new StageBuilder<T>(new PipelineBuilder());
        }

        internal void AddSourcePlan<TStage, TResult>(StageWithResultExecutionPlan<TStage, TResult> plan)
        {
            if (plan == null) throw new ArgumentNullException(nameof(plan));
            stagePlans.Add(plan);
        }

        internal void AddStagePlan<TStage, TInput, TResult>(StageWithInputAndResultExecutionPlan<TStage, TInput, TResult> plan)
        {
            if (plan == null) throw new ArgumentNullException(nameof(plan));
            stagePlans.Add(plan);
        }

        internal void AddStagePlan<TStage, TResult>(StageWithInputExecutionPlan<TStage, TResult> plan)
        {
            if (plan == null) throw new ArgumentNullException(nameof(plan));
            stagePlans.Add(plan);
        }

        public StageBuilder<TStage> ThenAddStage<TStage>()
        {
            return new StageBuilder<TStage>(this);
        }

        public Pipeline Build()
        {
            var pipeline = new Pipeline();
            foreach (var stagePlan in stagePlans)
                pipeline.AddStage(stagePlan.BuildPerformer());
            return pipeline;
        }
    }
}