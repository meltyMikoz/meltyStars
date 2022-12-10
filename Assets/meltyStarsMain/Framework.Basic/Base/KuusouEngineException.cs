using System;

namespace KuusouEngine
{
    public class KuusouEngineException : Exception
    {
        public KuusouEngineException() { }
        public KuusouEngineException(string message) : base(message) { }
    }
}
