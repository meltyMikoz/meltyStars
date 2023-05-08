using KuusouEngine.EngineBasic.Event;
using System;

namespace KuusouEngine.EngineImplement.Event
{
    public abstract class AbstractEventHandler<TEvent> : IEventHandler where TEvent : struct, IEvent
    {
        public Type EventType
        {
            get
            {
                return typeof(TEvent);
            }
        }

        public void Handle(object sender, IEvent eventArgs)
        {
            try
            {
                InternalHandle(sender, (TEvent)eventArgs);
            }
            catch (Exception e)
            {
                KuusouEngineLog.Error(e);
            }
        }

        protected abstract void InternalHandle(object sender, TEvent eventArgs);

        public virtual void OnSubscribe(IEventProxyOwner owner) { }

        public virtual void OnUnsubscribe(IEventProxyOwner owner) { }

        public virtual void Clear() { }
    }
}
