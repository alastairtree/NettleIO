using System.Threading.Tasks;

namespace NettleIO.Core
{
    public interface ISource
    {
        Task<IValueResult<object>> RecieveAsync();
    }
}