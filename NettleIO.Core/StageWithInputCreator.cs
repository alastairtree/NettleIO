using System;

namespace NettleIO.Core
{
    public class StageWithInputCreator<TInput>
    {
        private readonly PipelineBuilder pipelineBuilder;

        public StageWithInputCreator(PipelineBuilder pipelineBuilder)
        {
            this.pipelineBuilder = pipelineBuilder ?? throw new ArgumentNullException(nameof(pipelineBuilder));
        }

        public StageBuilder<TStage, TInput> ThenAddStage<TStage>()
        {
            return new StageBuilder<TStage, TInput>(pipelineBuilder);
        }
    }
}