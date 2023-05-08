namespace KuusouEngine
{
    public static partial class KuusouEngineLog
    {
        public interface ILogHelper
        {
            void Log(KuusouEngineLogLevel level, object message);
        }
    }
}
