using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace NettleIO.Core
{
    public interface IPipeline
    {
        IPipeline RegisterSource<TSource>() where TSource : ISource;

        IPipeline AddStage<TStage,TDataIn,TDataOut>() where TStage : IStage<TDataIn,TDataOut>;

        //IPipeline AddStage<TStage, TDataIn, TDataOut>(Expression<Func<TStage,TDataIn,Task<IValueResult<TDataOut>>>> executeExpression) where TStage : IStage<TDataIn, TDataOut>;

        //IPipeline AddStage<TStage, TData>(Expression<Func<TStage,TData, Task<IValueResult<TData>>>> executeExpression) where TStage : IStage<TData>;


        IPipeline AddStage<TStage, TData>() where TStage : IStage<TData>;
        IPipeline RegisterDestination<TDestination, TData>() where TDestination : IDestination<TData>;

        Task Execute();
    }
}