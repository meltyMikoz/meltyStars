namespace KuusouEngine
{
    public static partial class KuusouEngineLog
    {
        private static ILogHelper s_logHelper;
        public static void SetLogHelper(ILogHelper logHelper)
        {
            s_logHelper = logHelper;
        }
        public static void Debug(object message) 
        {
            s_logHelper.Log(KuusouEngineLogLevel.Debug, message);
        }
        public static void Info(object message)
        {
            s_logHelper.Log(KuusouEngineLogLevel.Info, message);
        }
        public static void Warning(object message)
        {
            s_logHelper.Log(KuusouEngineLogLevel.Warning, message);
        }
        public static void Error(object message)
        {
            s_logHelper.Log(KuusouEngineLogLevel.Error, message);
        }
        public static void Fatal(object message)
        {
            s_logHelper.Log(KuusouEngineLogLevel.Fatal, message);
        }
    }
}
