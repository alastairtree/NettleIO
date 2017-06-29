using System.Threading.Tasks;

namespace NettleIO.Core
{
    public interface IDestination<in TData> : IDestination
    {
        Task<IActionResult> SendAsync(TData item);
    }

    public interface IDestination
    {
        //TODO: try and remove the need for objects! can we use expressions lamdas and generics to make this more friendly?!
        Task<IActionResult> SendAsync(object item);
    }
}