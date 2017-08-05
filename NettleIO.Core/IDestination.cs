using System.Threading.Tasks;

namespace NettleIO.Core
{
    public interface IDestination<in TData> 
    {
        Task<IStageResult> SendAsync(TData item);
    }
}