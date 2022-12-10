using System;

namespace KuusouEngine
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class EventAttribute : AttributeBase
    {
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AutoSubscribeEventHandlerAttribute : AttributeBase
    {
    }
}
