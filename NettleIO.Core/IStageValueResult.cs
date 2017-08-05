namespace NettleIO.Core
{
    public interface IStageValueResult<out TValue> : IStageResult
    {
        TValue Value { get; }
    }
}