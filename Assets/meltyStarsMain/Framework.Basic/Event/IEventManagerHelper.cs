using System;

namespace KuusouEngine.EngineBasic.Event
{
    /// <summary>
    /// 事件管理器辅助者接口
    /// </summary>
    public interface IEventManagerHelper
    {
        Type HashToType(int eventHashId);
        int TypeToHash(Type eventType);
    }
}
