using System;

namespace MeltyStars
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
