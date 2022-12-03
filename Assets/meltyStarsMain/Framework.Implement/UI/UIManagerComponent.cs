using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MeltyStars.UI
{
    public partial class UIManagerComponent : AComponent
    {
        public static UIManagerComponent Instance;
        private readonly Transform m_UIRoot;
        private readonly Dictionary<EUIRoot, Transform> m_SubUIRoot;
        private readonly Dictionary<Type, AUIWindow> m_UIWindows;
        /// <summary>
        /// 目前显示的UI窗口
        /// </summary>
        private readonly Dictionary<Type, AUIWindow> m_UIWindowsShow;
        /// <summary>
        /// 每一个事件所响应的UI窗口
        /// </summary>
        private readonly Dictionary<Type, Dictionary<AUIWindow, IUIEventHandler>> m_RegisteredEventWindows;
        /// <summary>
        /// 事件注册的Handler
        /// </summary>
        private readonly Dictionary<Type, IEventHandler> m_RegisteredEventHandlers;
        private UIManagerComponent()
        {
            m_UIRoot = ObjectScheduler.Instance.GetGameObject("UIRoot", "UIRoot").transform;
            m_SubUIRoot = new Dictionary<EUIRoot, Transform>();
            m_UIWindows = new Dictionary<Type, AUIWindow>();
            m_UIWindowsShow = new Dictionary<Type, AUIWindow>();
            m_RegisteredEventWindows = new Dictionary<Type, Dictionary<AUIWindow, IUIEventHandler>>();
            m_RegisteredEventHandlers = new Dictionary<Type, IEventHandler>();

            m_SubUIRoot.Add(EUIRoot.Normal, m_UIRoot.Find("Normal"));
            m_SubUIRoot.Add(EUIRoot.Fixed, m_UIRoot.Find("Fixed"));
            m_SubUIRoot.Add(EUIRoot.PopUp, m_UIRoot.Find("PopUp"));
            GameObject.DontDestroyOnLoad(m_UIRoot);
        }
    }
}
