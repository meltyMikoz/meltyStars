using System;
using System.Collections.Generic;

namespace KuusouEngine.UI
{
    public partial class UIManagerComponent
    {
        public class UIManagerComponentEventHandler<TEventType> : AEventHandler<TEventType> where TEventType : struct
        {
            protected override void Execute(TEventType eventType)
            {
                UIManagerComponent.Instance.UIDispatch(eventType);
            }
        }
        /// <summary>
        /// 分发UI事件
        /// </summary>
        public void UIDispatch<TEventType>(TEventType eventType) where TEventType : struct
        {
            Type type = typeof(TEventType);
            if (!m_RegisteredEventWindows.ContainsKey(type)) return;
            foreach (var windowHandlerKV in m_RegisteredEventWindows[type])
            {
                (windowHandlerKV.Value as IUIEventHandler<TEventType>).Handle(windowHandlerKV.Key, eventType);
            }
        }
        /// <summary>
        /// 注册UI事件
        /// </summary>
        public void RegisterEvent<TEventType>() where TEventType : struct
        {
            Type type = typeof(TEventType);
            if (!m_RegisteredEventHandlers.ContainsKey(type))
            {
                AEventHandler<TEventType> eventHandler = new UIManagerComponentEventHandler<TEventType>();
                m_RegisteredEventHandlers.Add(type, eventHandler);
                EventScheduler.Instance.Subscribe(eventHandler);
            }
        }
        /// <summary>
        /// 注册UI事件以及其响应窗口
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="window"></param>
        /// <param name="iUIEventHandler"></param>
        public void RegisterUIEvent(Type eventType, AUIWindow window, IUIEventHandler iUIEventHandler)
        {
            if (!m_RegisteredEventWindows.ContainsKey(eventType))
                m_RegisteredEventWindows.Add(eventType, new Dictionary<AUIWindow, IUIEventHandler>());
            m_RegisteredEventWindows[eventType].Add(window, iUIEventHandler);
        }
    }
}
