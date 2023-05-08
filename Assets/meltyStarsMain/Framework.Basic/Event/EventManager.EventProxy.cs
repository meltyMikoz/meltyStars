using System.Collections.Concurrent;
using System.Collections.Generic;
using System;

namespace KuusouEngine.EngineBasic.Event
{
    internal sealed partial class EventManager
    {
        /// <summary>
        /// 事件代理
        /// </summary>
        public class EventProxy : IEventProxy
        {
            protected IEventProxyOwner _owner;
            protected IEventManager _eventManager;
            protected Type _ownerType;
            private readonly ConcurrentDictionary<Type, List<IEventHandler>> _eventHandlers;

            public EventProxy()
            {
                _eventHandlers = new ConcurrentDictionary<Type, List<IEventHandler>>();
            }

            public virtual Type OwnerType
            {
                get 
                { 
                    return _ownerType; 
                }
            }

            public IEventManager EventManager
            {
                get
                {
                    return _eventManager;
                }
            }

            public IEventProxyOwner Owner
            {
                get 
                {
                    return _owner;
                }
            }

            public static EventProxy Create(IEventProxyOwner owner, IEventManager eventManager)
            {
                EventProxy eventProxy = ReferencePool.Fetch<EventProxy>();
                eventProxy._owner = owner;
                eventProxy._eventManager = eventManager;
                eventProxy._ownerType = owner.GetType();
                return eventProxy;
            }

            public void Clear()
            {
                _owner = null;
                _eventManager = null;
                _eventHandlers.Clear();
            }

            public void Subscribe(IEventHandler eventHandler)
            {
                if (eventHandler is null)
                {
                    throw new KuusouEngineException("Event handler is invalid.");
                }
                if (!_eventHandlers.ContainsKey(eventHandler.EventType))
                {
                    if (!_eventHandlers.TryAdd(eventHandler.EventType, new List<IEventHandler>()))
                    {
                        throw new KuusouEngineException($"Event type {eventHandler.EventType.FullName} can not add to the dictionary.");
                    }
                }
                if (_eventHandlers[eventHandler.EventType].Contains(eventHandler))
                {
                    throw new KuusouEngineException("You are trying to add a same reference! This is not allowed.");
                }
                _eventHandlers[eventHandler.EventType].Add(eventHandler);
                eventHandler.OnSubscribe(_owner);
            }

            public void Unsubscribe(IEventHandler eventHandler)
            {
                if (!_eventHandlers.ContainsKey(eventHandler.EventType))
                {
                    throw new KuusouEngineException($"No events of this type {eventHandler.EventType.FullName} are subscribed.");

                }
                if (!_eventHandlers[eventHandler.EventType].Contains(eventHandler))
                {
                    throw new KuusouEngineException("You are trying to remove a reference which is not registered! This is not allowed.");
                }
                _eventHandlers[eventHandler.EventType].Remove(eventHandler);
                if (_eventHandlers[eventHandler.EventType].Count == 0)
                {
                    if (!_eventHandlers.TryRemove(eventHandler.EventType, out List<IEventHandler> handlers))
                    {
                        throw new KuusouEngineException($"Event type {eventHandler.EventType.FullName} can not remove from the dictionary. {handlers.Count} handlers of this type are remained.");
                    }
                }
                eventHandler.OnUnsubscribe(_owner);
            }

            public void UnsubscribeType(Type eventType)
            {
                if (!typeof(IEvent).IsAssignableFrom(eventType))
                {
                    throw new KuusouEngineException("Event type is invalid.");
                }
                InternalUnsubscribeType(eventType);
            }

            private void InternalUnsubscribeType(Type eventType)
            {
                if (!_eventHandlers.ContainsKey(eventType))
                {
                    throw new KuusouEngineException($"No events of this type {eventType.FullName} are subscribed.");
                }
                List<IEventHandler> handlers;
                if (!_eventHandlers.TryRemove(eventType, out handlers))
                {
                    throw new KuusouEngineException($"Event type {eventType.FullName} can not remove from the dictionary. {handlers.Count} handlers of this type are remained.");
                }
                foreach (IEventHandler handler in handlers)
                {
                    handler.OnUnsubscribe(_owner);
                }
            }

            public void UnsubscribeAll()
            {
                IEnumerable<Type> eventTypes = _eventHandlers.Keys;
                foreach (Type eventType in eventTypes)
                {
                    InternalUnsubscribeType(eventType);
                }
            }

            public void Publish(Type eventType, object sender, IEvent eventArgs)
            {
                if (!_eventHandlers.ContainsKey(eventType))
                {
                    KuusouEngineLog.Warning($"No events of this type {eventType.FullName} are subscribed, but you are trying to publish it on an owner {_owner.GetType().FullName}.");
                    return;
                }
                lock (_eventHandlers)
                {
                    foreach (IEventHandler handler in _eventHandlers[eventType])
                    {
                        try
                        {
                            handler.Handle(sender, eventArgs);
                        }
                        catch (Exception e)
                        {
                            KuusouEngineLog.Error(e);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 泛型事件代理
        /// </summary>
        /// <typeparam name="TOwner">代理拥有者</typeparam>
        public class EventProxy<TOwner> : EventProxy, IEventProxy<TOwner> where TOwner : class, IEventProxyOwner
        {
            public new TOwner Owner
            {
                get
                {
                    return _owner as TOwner;
                }
            }

            public static EventProxy<TOwner> Create(TOwner owner, IEventManager eventManager)
            {
                EventProxy<TOwner> eventProxy = ReferencePool.Fetch<EventProxy<TOwner>>();
                eventProxy._owner = owner;
                eventProxy._eventManager = eventManager;
                eventProxy._ownerType = typeof(TOwner);
                return eventProxy;
            }
        }
    }
}
