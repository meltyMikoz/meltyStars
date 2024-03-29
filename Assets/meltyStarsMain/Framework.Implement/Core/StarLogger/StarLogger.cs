using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace KuusouEngine
{
    /// <summary>
    /// 扩展日志
    /// </summary>
    public class StarLogger
    {
        private static string GetLogStr(object logItem, string color)
        {
            return $"[<b><color=#9966ff>MSLogger</color></b>] {logItem.ToString().ToRichText(color)}";
        }
        [Conditional("ENABLE_MSLOGGER")]
        public static void LogInfo(object logItem, string color = RichTextUtil.Green)
        {
            Debug.Log(GetLogStr(logItem, color));
        }
        [Conditional("ENABLE_MSLOGGER")]
        public static void LogWarning(object logItem)
        {
            Debug.LogWarning(GetLogStr(logItem, RichTextUtil.Oringe));
        }
        [Conditional("ENABLE_MSLOGGER")]
        public static void LogError(object logItem)
        {
            Debug.LogError(GetLogStr(logItem, RichTextUtil.Red));
        }
    }
}