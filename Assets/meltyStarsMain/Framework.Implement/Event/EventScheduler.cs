using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace KuusouEngine
{
    public class EventScheduler : SingletonFor<EventScheduler>
    {
        private Dictionary<Type, List<Type>> m_types;
        private Dictionary<Type, List<IEventHandler>> m_allEventHandlers;
        private EventScheduler() { AutoSubscribe(); }
        public void Dispatch<TEventType>(TEventType eventType) where TEventType : struct
        {
            List<IEventHandler> iEventHandlers;
            if (!m_allEventHandlers.TryGetValue(typeof(TEventType), out iEventHandlers))
                return;
            iEventHandlers.ForEach(iEventHandler =>
            {
                try
                {
                    if (iEventHandler is AEventHandler<TEventType> aEventHandler)
                        aEventHandler.Handle(eventType);
                }
                catch (Exception e)
                {

                    StarLogger.LogError(e);
                }
            });
        }
        /// <summary>
        /// 订阅事件
        /// </summary>
        public AEventHandler<TEventType> Subscribe<TEventType>(AEventHandler<TEventType> handler) where TEventType : struct
        {
            Type eventType = typeof(TEventType);
            if (!m_allEventHandlers.ContainsKey(eventType))
            {
                m_allEventHandlers.Add(eventType, new List<IEventHandler>());
            }
            m_allEventHandlers[eventType].Add(handler);
            return handler;
        }
        public void Subscribe(IEventHandler handler)
        {
            Type eventType = handler.GetTypeOfEvent();
            if (!m_allEventHandlers.ContainsKey(eventType))
            {
                m_allEventHandlers.Add(eventType, new List<IEventHandler>());
            }
            m_allEventHandlers[eventType].Add(handler);
        }
        /// <summary>
        /// 注销订阅
        /// </summary>
        public void UnSubscribe<TEventType>(AEventHandler<TEventType> handler) where TEventType : struct
        {
            Type eventType = typeof(TEventType);
            if (!m_allEventHandlers.ContainsKey(eventType)) return;
            if (!m_allEventHandlers[eventType].Contains(handler)) return;
            m_allEventHandlers[eventType].Remove(handler);
        }
        public void UnSubscribe(IEventHandler handler)
        {
            Type eventType = handler.GetTypeOfEvent();
            if (!m_allEventHandlers.ContainsKey(eventType)) return;
            if (!m_allEventHandlers[eventType].Contains(handler)) return;
            m_allEventHandlers[eventType].Remove(handler);
        }
        /// <summary>
        /// 自动订阅带有AutoSubscribe特性的事件
        /// </summary>
        private void AutoSubscribe()
        {
            m_types = new Dictionary<Type, List<Type>>();
            m_allEventHandlers = new Dictionary<Type, List<IEventHandler>>();
            List<Type> attributes = GetAttributeBase();
            foreach (var attributeBase in attributes)
            {
                foreach (var kv in App.Instance.GetAllTypes())
                {
                    Type type = kv.Value;
                    if (type.IsAbstract) continue;

                    var objects = type.GetCustomAttributes(attributeBase, true);
                    if (objects.Length == 0) continue;
                    if (!m_types.ContainsKey(attributeBase))
                    {
                        m_types.Add(attributeBase, new List<Type>());
                    }
                    m_types[attributeBase].Add(type);
                }
            }
            foreach (var type in m_types[typeof(EventAttribute)])
            {
                var types = type.GetCustomAttributes(typeof(AutoSubscribeEventHandlerAttribute), true);
                if (types.Length == 0) continue;
                IEventHandler iEventHandler = Activator.CreateInstance(type) as IEventHandler;
                if (iEventHandler != null)
                {
                    Type eventType = iEventHandler.GetTypeOfEvent();
                    if (!m_allEventHandlers.ContainsKey(eventType))
                    {
                        m_allEventHandlers.Add(eventType, new List<IEventHandler>());
                    }
                    m_allEventHandlers[eventType].Add(iEventHandler);
                }
            }
        }
        private List<Type> GetAttributeBase()
        {
            List<Type> attributes = new List<Type>();
            foreach (var kv in App.Instance.GetAllTypes())
            {
                Type type = kv.Value;
                if (type.IsAbstract) continue;
                if (type.IsSubclassOf(typeof(AttributeBase))) attributes.Add(type);
            }
            return attributes;
        }
    }
}
