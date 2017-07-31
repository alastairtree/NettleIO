using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace NettleIO.Core
{
    public interface IPipeline
    {
        IPipeline AddSource<TSource,TDataOut>() where TSource : ISource<TDataOut>;

        IPipeline AddStage<TStage,TDataIn,TDataOut>() where TStage : IStage<TDataIn,TDataOut>;

        IPipeline AddStage<TStage, TData>() where TStage : IStage<TData>;
        IPipeline RegisterDestination<TDestination, TData>() where TDestination : IDestination<TData>;

        Task<PipelineExecutionReport> Execute();
    }
}