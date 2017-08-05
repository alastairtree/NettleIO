using System.Threading.Tasks;

namespace NettleIO.Core
{
    public interface ISource<TResult>
    {
        Task<IStageValueResult<TResult>> RecieveAsync();
    }
}