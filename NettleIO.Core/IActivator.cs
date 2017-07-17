using System;

namespace NettleIO.Core
{
    public interface IActivator
    {
        object Create(Type type);
        T Create<T>();
    }
}