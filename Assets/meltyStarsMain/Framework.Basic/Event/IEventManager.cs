using System;

namespace KuusouEngine.EngineBasic.Event
{
    /// <summary>
    /// 事件管理器接口
    /// </summary>
    public interface IEventManager
    {
        IEventProxy GetEventProxy(IEventProxyOwner owner);
        IEventProxy<TOwner> GetEventProxy<TOwner>(TOwner owner) where TOwner : class, IEventProxyOwner;
        void RecycleEventProxy(IEventProxyOwner owner);
        void RecycleEventProxy(IEventProxy eventProxy);
        void Subscribe(IEventHandler eventHandler, IEventProxyOwner owner);
        void Unsubscribe(IEventHandler eventHandler, IEventProxyOwner owner);
        void UnsubscribeType(Type eventType, IEventProxyOwner owner);
        void UnsubscribeAll(IEventProxyOwner owner);
        void Publish(int eventId, object sender, IEvent eventArgs, IEventProxyOwner owner);
        void Publish<TEvent>(object sender, TEvent eventArgs, IEventProxyOwner owner) where TEvent : struct, IEvent;
        void PublishImmediately(int eventId, object sender, IEvent eventArgs, IEventProxyOwner owner);
        void PublishImmediately<TEvent>(object sender, TEvent eventArgs, IEventProxyOwner owner) where TEvent : struct, IEvent;
        void SetEventManagerHelper(IEventManagerHelper eventManagerHelper);
    }
}
