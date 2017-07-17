using System;

namespace NettleIO.Core
{
    public class Activator : IActivator
    {
        public object Create(Type type)
        {
            return System.Activator.CreateInstance(type);
        }

        public T Create<T>()
        {
            return System.Activator.CreateInstance<T>();
        }
    }
}