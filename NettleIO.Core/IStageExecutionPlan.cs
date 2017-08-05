namespace NettleIO.Core
{
    internal interface IStageExecutionPlan
    {
        IStagePerformer BuildPerformer();
    }
}