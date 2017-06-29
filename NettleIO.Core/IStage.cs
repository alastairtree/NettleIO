using System;
using System.Threading.Tasks;

namespace NettleIO.Core
{
    public interface IStage
    {
        //TODO: try and remove the need for objects! can we use expressions lamdas and generics to make this more friendly?!
        Task<IValueResult<object>> Execute(object input);
    }

    //TODO: Is this just a Func<in T, out TRestult> ?!
    public interface IStage<TDataIn, TDataOut> : IStage
    {
        Task<IValueResult<TDataOut>> Execute(TDataIn input);
    }

    public interface IStage<TData> : IStage<TData,TData>
    {

    }
}