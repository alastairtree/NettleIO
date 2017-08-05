using System;
using System.Reflection;
using System.Threading.Tasks;

namespace NettleIO.Core.Exceptions
{
    public class StageReturnsTheWrongTypeException : Exception
    {
        public StageReturnsTheWrongTypeException(MethodInfo method, Type foundType) 
            : base((string) $"Invoking {method} must return a {typeof(Task<IStageResult>)} but instead it returns {foundType}. Change the methods return type.")
        {
            throw new NotImplementedException();
        }
    }
}