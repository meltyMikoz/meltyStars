using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KuusouEngine
{
    public partial class InputScheduler : SingletonFor<InputScheduler>
    {
        private GameObject m_InputObserver;
        private OnKeyDownInputHandler m_OnKeyDownInputHandler;
        private InputScheduler()
        {
            EventScheduler.Instance.Subscribe(new OnKeyDownInputHandler() { Action = OnKeyDownInputHandle });
            EventScheduler.Instance.Subscribe(new OnKeyUpInputHandler() { Action = OnKeyUpInputHandle });
            GameObject.DontDestroyOnLoad(m_InputObserver = new GameObject(nameof(InputObserverMono)));
            m_InputObserver.AddComponent<InputObserverMono>();
        }
        private void OnKeyDownInputHandle(OnKeyDown eventType)
        {
            Debug.Log($"Key Down : {eventType.Key}");
        }
        private void OnKeyUpInputHandle(OnKeyUp eventType)
        {
            Debug.Log($"Key  Up  : {eventType.Key}");
        }
    }
    public partial class InputScheduler
    {
        public struct OnKeyDown
        {
            public KeyCode Key;
        }
        public struct OnKeyUp
        {
            public KeyCode Key;
        }
    }
    public partial class InputScheduler
    {
        private class OnKeyDownInputHandler : AEventHandler<OnKeyDown>
        {
            public Action<OnKeyDown> Action;
            protected override void Execute(OnKeyDown eventType)
            {
                Action?.Invoke(eventType);
            }
        }
        private class OnKeyUpInputHandler : AEventHandler<OnKeyUp>
        {
            public Action<OnKeyUp> Action;
            protected override void Execute(OnKeyUp eventType)
            {
                Action?.Invoke(eventType);
            }
        }
    }
}
