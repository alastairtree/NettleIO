using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NettleIO.Core
{
    public class Pipeline : IPipeline
    {
        private IActivator activator;

        private readonly List<IStagePerformer> performers = new List<IStagePerformer>();

        public Pipeline() : this(new Activator())
        {
        }

        public Pipeline(IActivator activator)
        {
            this.activator = activator;
        }

        public IPipeline AddSource<TSource, TDataOut>() where TSource : ISource<TDataOut>
        {
            performers.Add(StagePerformer<TSource, TDataOut>.Build(stage => stage.RecieveAsync()));
            return this;
        }

        public IPipeline AddStage<TStage, TDataIn, TDataOut>() where TStage : IStage<TDataIn, TDataOut>
        {
            performers.Add(StagePerformer<TStage, TDataOut>.Build<TDataIn>((stage, input) => stage.Execute(input)));
            return this;
        }

        public IPipeline AddStage<TStage, TData>() where TStage : IStage<TData>
        {
            return AddStage<TStage, TData, TData>();
        }

        public IPipeline RegisterDestination<TDestination, TData>() where TDestination : IDestination<TData>
        {
            performers.Add(StagePerformer<TDestination>.Build<TData>((stage, input) => stage.SendAsync(input)));
            return this;
        }

        public async Task<PipelineExecutionReport> Execute()
        {
            Validate();

            var results = new List<IActionResult>();
            object value = null;
            foreach (var performer in performers)
            {
                performer.PrepareToPerform(activator);
                if (value != null)
                {
                    await performer.Perform(value);
                }
                else
                {
                    await performer.Perform();
                }
                results.Add(performer
                    .Result); //TODO: We dont need all the result Value data, just the metadata. clone to another object?
                value = performer.Value;
            }

            Console.WriteLine("Completed!");

            return new PipelineExecutionReport(results);
        }

        public void SetActivator(IActivator activator)
        {
            this.activator = activator ?? throw new ArgumentNullException(nameof(activator));
        }

        protected virtual void Validate()
        {
            Console.WriteLine("Doing some validation....?");
        }

        public void AddStage(IStagePerformer stagePerformer)
        {
            if (stagePerformer == null) throw new ArgumentNullException(nameof(stagePerformer));
            performers.Add(stagePerformer);
        }
    }
}