using System.Threading.Tasks;

namespace NettleIO.Core
{

    public interface IStagePerformer<out TStage> : IStagePerformer
    {
        TStage StageInstance { get; }

    }
    public interface IStagePerformer
    {
        object Value { get; }
        void PrepareToPerform(IActivator activator);
        Task Perform(params object[] arguments);
        IStageResult Result { get; }
    }
}