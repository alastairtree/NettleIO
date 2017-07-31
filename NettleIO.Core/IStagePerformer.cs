using System.Threading.Tasks;

namespace NettleIO.Core
{
    public interface IStagePerformer
    {
        IActionResult Result { get; }
        object Value { get; }
        void PrepareToPerform(IActivator activator);
        Task Perform(params object[] arguments);
    }
}