using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace MeltyStars.UI
{
    public partial class UIManagerComponent
    {
        public async UniTask ShowWindow<T>(EUIRoot root = EUIRoot.Normal, object args = null) where T : AUIWindow
        {
            Type windowType = typeof(T);
            await ShowWindowCore(windowType, root, args);
        }
        public async UniTask ShowWindow(Type windowType, EUIRoot root = EUIRoot.Normal, object args = null)
        {
            if (!windowType.IsSubclassOf(typeof(AUIWindow)))
            {
                StarLogger.LogError($"ShowWindow(Type) is error : {windowType.ToString()}");
            }
            await ShowWindowCore(windowType, root, args);
        }
        private async UniTask ShowWindowCore(Type windowType, EUIRoot root, object args)
        {
            if (!m_UIWindows.ContainsKey(windowType))
            {
                RegisterWindow(windowType);
            }
            AUIWindow window = m_UIWindows[windowType];
            window.SetParent(m_SubUIRoot[root]);
            window.SetAsLastSibling();
            await window.OnShow(args);
            m_UIWindowsShow.Add(windowType, window);
        }
        public async UniTask HideWindow<T>() where T : AUIWindow
        {
            Type windowType = typeof(T);
            await HideWindowCore(windowType);
        }
        public async UniTask HideWindow(Type windowType)
        {
            if (!windowType.IsSubclassOf(typeof(AUIWindow)))
            {
                StarLogger.LogError($"HideWindow(Type) is error : {windowType.ToString()}");
            }
            await HideWindowCore(windowType);
        }
        private async UniTask HideWindowCore(Type windowType)
        {
            if (!m_UIWindowsShow.ContainsKey(windowType)) return;
            await m_UIWindowsShow[windowType].OnHide();
            m_UIWindowsShow.Remove(windowType);
        }
        /// <summary>
        /// 注册窗口
        /// </summary>
        /// <param name="windowType"></param>
        private void RegisterWindow(Type windowType)
        {
            AUIWindow createdWindow = Activator.CreateInstance(windowType) as AUIWindow;
            createdWindow.OnInit();
            RegisterWindowEvent(createdWindow);
            m_UIWindows.Add(windowType, createdWindow);
        }
        /// <summary>
        /// 注册事件的响应窗口
        /// </summary>
        /// <param name="window"></param>
        /// <param name="eventTypes"></param>
        private void RegisterWindowEvent(AUIWindow window)
        {
            var types = window.GetType().GetNestedTypes();
            foreach (var type in types)
            {
                if (type.IsAbstract) continue;
                var uiEvent = type.GetCustomAttributes(typeof(UIEventAttribute), true);
                if (uiEvent.Length == 0) continue;
                IUIEventHandler iUIEventHandler = Activator.CreateInstance(type) as IUIEventHandler;
                if (object.ReferenceEquals(iUIEventHandler, null))
                {
                    StarLogger.LogError($"{type.ToString()} 转换失败");
                    continue;
                }
                iUIEventHandler.Register(window);
            }
        }
    }
}
