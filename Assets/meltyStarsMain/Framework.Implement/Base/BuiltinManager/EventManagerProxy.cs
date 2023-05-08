using KuusouEngine.EngineBasic.Event;
using System;

namespace KuusouEngine.EngineImplement
{
    public class EventManagerProxy : BuiltinManagerProxy<IEventManager>, IEventProxyOwner
    {
        private IEventProxy<EventManagerProxy> _defaultEventProxy;

        public IEventProxy EventProxy
        {
            get 
            {
                return _defaultEventProxy;
            }
        }

        protected override void OnInit()
        {
            _defaultEventProxy = _manager.GetEventProxy(this);
        }

        public IEventProxy GetEventProxy(IEventProxyOwner owner)
        {
            return _manager.GetEventProxy(owner);
        }

        public IEventProxy<TOwner> GetEventProxy<TOwner>(TOwner owner)
            where TOwner : class, IEventProxyOwner
        {
            return _manager.GetEventProxy(owner);
        }

        public void Publish(int eventId, object sender, IEvent eventArgs, IEventProxyOwner owner)
        {
            _manager.Publish(eventId, sender, eventArgs, owner);
        }

        public void Publish<TEvent>(object sender, TEvent eventArgs, IEventProxyOwner owner) 
            where TEvent : struct, IEvent
        {
            _manager.Publish(sender, eventArgs, owner);
        }

        public void PublishImmediately(int eventId, object sender, IEvent eventArgs, IEventProxyOwner owner)
        {
            _manager.PublishImmediately(eventId, sender, eventArgs, owner);
        }

        public void PublishImmediately<TEvent>(object sender, TEvent eventArgs, IEventProxyOwner owner) 
            where TEvent : struct, IEvent
        {
            _manager.PublishImmediately(sender, eventArgs, owner);
        }

        public void RecycleEventProxy(IEventProxyOwner owner)
        {
            _manager.RecycleEventProxy(owner);
        }

        public void RecycleEventProxy(IEventProxy eventProxy)
        {
            _manager.RecycleEventProxy(eventProxy);
        }

        public void SetEventManagerHelper(IEventManagerHelper eventManagerHelper)
        {
            _manager.SetEventManagerHelper(eventManagerHelper);
        }

        public void Subscribe(IEventHandler eventHandler, IEventProxyOwner owner)
        {
            _manager.Subscribe(eventHandler, owner);
        }

        public void Unsubscribe(IEventHandler eventHandler, IEventProxyOwner owner)
        {
            _manager.Unsubscribe(eventHandler, owner);
        }

        public void UnsubscribeAll(IEventProxyOwner owner)
        {
            _manager.UnsubscribeAll(owner);
        }

        public void UnsubscribeType(Type eventType, IEventProxyOwner owner)
        {
            _manager.UnsubscribeType(eventType, owner);
        }

        public void UnsubscribeType<TEvent>(IEventProxyOwner owner) where TEvent : struct, IEvent
        {
            UnsubscribeType(typeof(TEvent), owner);
        }

        //以下是使用默认Proxy的方法

        public void Subcribe(IEventHandler eventHandler)
        {
            _manager.Subscribe(eventHandler, this);
        }

        public void Unsubscribe(IEventHandler eventHandler)
        {
            _manager.Unsubscribe(eventHandler, this);
        }

        public void Publish(int eventId, object sender, IEvent eventArgs)
        {
            _manager.Publish(eventId, sender, eventArgs, this);
        }

        public void Publish<TEvent>(object sender, TEvent eventArgs)
            where TEvent : struct, IEvent
        {
            _manager.Publish(sender, eventArgs, this);
        }

        public void PublishImmediately(int eventId, object sender, IEvent eventArgs)
        {
            _manager.PublishImmediately(eventId, sender, eventArgs, this);
        }

        public void PublishImmediately<TEvent>(object sender, TEvent eventArgs)
            where TEvent : struct, IEvent
        {
            _manager.PublishImmediately(sender, eventArgs, this);
        }

        public void UnsubscribeType<TEvent>() where TEvent : struct, IEvent
        {
            UnsubscribeType(typeof(TEvent), this);
        }
    }
}
