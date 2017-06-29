namespace NettleIO.Core
{
    public interface IValueResult<out TValue> : IActionResult
    {
        TValue Value { get; }
    }
}