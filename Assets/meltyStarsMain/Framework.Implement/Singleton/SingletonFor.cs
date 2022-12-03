using System;

namespace MeltyStars
{
    public abstract class SingletonFor<T> where T : class
    {
        public static T Instance => Inner.instance;
        private class Inner
        {
            internal static readonly T instance = Activator.CreateInstance(typeof(T), true) as T;
        }
    }
}