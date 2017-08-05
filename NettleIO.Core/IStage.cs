using System;
using System.Threading.Tasks;

namespace NettleIO.Core
{
    //TODO: Is this just a Func<in T, out TRestult> ?!
    public interface IStage<TDataIn, TDataOut>
    {
        Task<IStageValueResult<TDataOut>> Execute(TDataIn input);
    }

    public interface IStage<TData> : IStage<TData,TData>
    {

    }
}