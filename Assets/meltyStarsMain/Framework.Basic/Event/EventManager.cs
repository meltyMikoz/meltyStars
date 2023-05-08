using Codice.Client.BaseCommands;
using System;
using System.Collections.Generic;
using static UnityEngine.UI.GridLayoutGroup;

namespace KuusouEngine.EngineBasic.Event
{
    internal sealed partial class EventManager : KuusouEngineBasicModule, IEventManager
    {
        private IEventManagerHelper _eventManagerHelper;
        private readonly Dictionary<IEventProxyOwner, IEventProxy> _eventProxyMap;
        private readonly Queue<EventInfo> _eventQueue;
        public EventManager() 
        {
            _eventProxyMap = new Dictionary<IEventProxyOwner, IEventProxy>();
            _eventQueue = new Queue<EventInfo>();
        }

        internal override int Priority
        {
            get 
            {
                return 0;
            }
        }

        internal override void ShutDown()
        {
            foreach (KeyValuePair<IEventProxyOwner, IEventProxy> kv in _eventProxyMap)
            {
                kv.Value.Clear();
                
            }
            _eventProxyMap.Clear();
        }

        internal override void Update(float elapseFrequency, float elapseFrequencyReally)
        {
            if (_eventQueue.Count == 0)
            {
                return;
            }
            HandleEvent(_eventQueue.Dequeue());
        }

        public IEventProxy GetEventProxy(IEventProxyOwner owner)
        {
            Type ownerType = owner.GetType();
            if (!ownerType.IsClass)
            {
                throw new KuusouEngineException($"Event proxy owner must be a class! But {ownerType.FullName} is not.");
            }
            return InternalGetEventProxy(owner);
        }

        public IEventProxy<TOwner> GetEventProxy<TOwner>(TOwner owner) 
            where TOwner : class, IEventProxyOwner
        {
            if (owner is null)
            {
                throw new KuusouEngineException("Owner is invalid");
            }
            return InternalGetEventProxyGeneric(owner);
        }

        private IEventProxy InternalGetEventProxy(IEventProxyOwner owner)
        {
            IEventProxy proxy = null;
            if (!_eventProxyMap.ContainsKey(owner))
            {
                proxy = EventProxy.Create(owner, this);
                _eventProxyMap.Add(owner, proxy);
                return proxy;
            }
            if (_eventProxyMap.TryGetValue(owner, out IEventProxy eventProxy))
            {
                proxy = eventProxy; ;
            }
            return proxy;
        }

        private IEventProxy<TOwner> InternalGetEventProxyGeneric<TOwner>(TOwner owner) 
            where TOwner : class, IEventProxyOwner
        {
            IEventProxy<TOwner> proxy = null;
            if (!_eventProxyMap.ContainsKey(owner))
            {
                proxy = EventProxy<TOwner>.Create(owner, this);
                _eventProxyMap.Add(owner, proxy);
                return proxy;
            }
            if (_eventProxyMap.TryGetValue(owner, out IEventProxy eventProxy))
            {
                proxy = eventProxy as IEventProxy<TOwner>;
            }
            return proxy;
        }

        private bool InternalTryGetEventProxy(IEventProxyOwner owner, out IEventProxy eventProxy) 
        {
            if (_eventProxyMap.TryGetValue(owner, out IEventProxy proxy))
            {
                eventProxy = proxy;
                return true;
            }
            eventProxy = null;
            return false;
        }

        public void RecycleEventProxy(IEventProxyOwner owner)
        {
            if (owner is null)
            {
                throw new KuusouEngineException("Owner is invalid");
            }
            InternalRecycleEventProxy(owner);
        }

        public void RecycleEventProxy(IEventProxy eventProxy) 
        {
            if (eventProxy is null)
            {
                throw new KuusouEngineException("Event proxy is invalid");
            }
            if (eventProxy.Owner is null)
            {
                throw new KuusouEngineException("Owner is invalid");
            }
            InternalRecycleEventProxy(eventProxy.Owner);
        }

        private void InternalRecycleEventProxy(IEventProxyOwner owner)
        {
            IEventProxy eventProxy;
            if (!InternalTryGetEventProxy(owner, out eventProxy))
            {
                KuusouEngineLog.Warning($"No proxy which owner type is {owner.GetType()} has been registered! but you are still trying to recycle it.");
                return;
            }
            _eventProxyMap.Remove(owner);
            eventProxy.Clear();
            ReferencePool.Release(eventProxy);
        }

        public void Publish(int eventId, object sender, IEvent eventArgs, IEventProxyOwner owner)
        {
            Type eventType = _eventManagerHelper.HashToType(eventId);
            InternalPublish(eventType, sender, eventArgs, owner);
        }

        public void Publish<TEvent>(object sender, TEvent eventArgs, IEventProxyOwner owner) 
            where TEvent : struct, IEvent
        {
            InternalPublish(typeof(TEvent), sender, eventArgs, owner);
        }

        private void InternalPublish(Type eventType, object sender, IEvent eventArgs, IEventProxyOwner owner)
        {
            EventInfo eventInfo = new EventInfo() 
            {
                EventType = eventType,
                Sender = sender,
                EventArgs = eventArgs,
                EventProxyOwner = owner
            };
            _eventQueue.Enqueue(eventInfo);
        }

        public void PublishImmediately(int eventId, object sender, IEvent eventArgs, IEventProxyOwner owner)
        {
            Type eventType = _eventManagerHelper.HashToType(eventId);
            InternalPublishImmediately(eventType, sender, eventArgs, owner);
        }

        public void PublishImmediately<TEvent>(object sender, TEvent eventArgs, IEventProxyOwner owner) 
            where TEvent : struct, IEvent
        {
            InternalPublishImmediately(typeof(TEvent), sender, eventArgs, owner);
        }

        private void InternalPublishImmediately(Type eventType, object sender, IEvent eventArgs, IEventProxyOwner owner)
        {
            EventInfo eventInfo = new EventInfo()
            {
                EventType = eventType,
                Sender = sender,
                EventArgs = eventArgs,
                EventProxyOwner = owner
            };
            HandleEvent(eventInfo);
        }

        public void Subscribe(IEventHandler eventHandler, IEventProxyOwner owner)
        {
            IEventProxy eventProxy;
            if (!InternalTryGetEventProxy(owner, out eventProxy))
            {
                KuusouEngineLog.Warning($"No proxy which owner type is {owner.GetType()} has been registered! But you are still trying to add subscriptions.");
            }
            eventProxy.Subscribe(eventHandler);
        }

        public void Unsubscribe(IEventHandler eventHandler, IEventProxyOwner owner)
        {
            IEventProxy eventProxy;
            if (!InternalTryGetEventProxy(owner, out eventProxy))
            {
                KuusouEngineLog.Warning($"No proxy which owner type is {owner.GetType()} has been registered! But you are still trying to remove subscriptions.");
            }
            eventProxy.Unsubscribe(eventHandler);
        }

        public void UnsubscribeType(Type eventType, IEventProxyOwner owner)
        {
            IEventProxy eventProxy;
            if (!InternalTryGetEventProxy(owner, out eventProxy))
            {
                KuusouEngineLog.Warning($"No proxy which owner type is {owner.GetType()} has been registered! But you are still trying to remove subscriptions.");
            }
            eventProxy.UnsubscribeType(eventType);
        }

        public void UnsubscribeAll(IEventProxyOwner owner)
        {
            IEventProxy eventProxy;
            if (!InternalTryGetEventProxy(owner, out eventProxy))
            {
                KuusouEngineLog.Warning($"No proxy which owner type is {owner.GetType()} has been registered! But you are still trying to remove subscriptions.");
            }
            eventProxy.UnsubscribeAll();
        }

        public void SetEventManagerHelper(IEventManagerHelper eventManagerHelper)
        {
            _eventManagerHelper = eventManagerHelper;
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="eventInfo"></param>
        private void HandleEvent(EventInfo eventInfo)
        {
            IEventProxy eventProxy;
            if (!InternalTryGetEventProxy(eventInfo.EventProxyOwner, out eventProxy))
            {
                KuusouEngineLog.Warning($"No proxy which owner type is {eventInfo.EventProxyOwner.GetType()} has been registered! but you are still trying to publish it.");
                return;
            }
            eventProxy.Publish(eventInfo.EventType, eventInfo.Sender, eventInfo.EventArgs);
        }
    }
}
