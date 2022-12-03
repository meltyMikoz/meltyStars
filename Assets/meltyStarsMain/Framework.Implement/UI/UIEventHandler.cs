using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MeltyStars.UI
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class UIEventAttribute : AttributeBase
    { }
    public interface IUIEventHandler
    {
        Type GetTypeOfEvent();
        void Register(AUIWindow window);
    }
    public interface IUIEventHandler<TEventType> : IUIEventHandler where TEventType : struct
    {
        void Handle(AUIWindow window, TEventType eventType);
    }
    [UIEvent]
    public abstract class AUIEventHandler<TEventType> : IUIEventHandler<TEventType> where TEventType : struct
    {
        public Type GetTypeOfEvent()
        {
            return typeof(TEventType);
        }
        protected abstract void Execute(AUIWindow window, TEventType eventType);
        public void Handle(AUIWindow window, TEventType eventType)
        {
            try
            {
                Execute(window, eventType);
            }
            catch (Exception e)
            {
                StarLogger.LogError(e);
            }
        }
        public void Register(AUIWindow window)
        {
            UIManagerComponent.Instance.RegisterEvent<TEventType>();
            UIManagerComponent.Instance.RegisterUIEvent(typeof(TEventType), window, this);
        }
    }
    [UIEvent]
    public abstract class AUIEventHandler<TEventType, TWindowType> : IUIEventHandler<TEventType> where TEventType : struct where TWindowType : AUIWindow
    {
        public Type GetTypeOfEvent()
        {
            return typeof(TEventType);
        }
        protected abstract void Execute(TWindowType window, TEventType eventType);
        public void Handle(AUIWindow window, TEventType eventType)
        {
            try
            {
                Execute(window as TWindowType, eventType);
            }
            catch (Exception e)
            {
                StarLogger.LogError(e);
            }
        }
        public void Register(AUIWindow window)
        {
            UIManagerComponent.Instance.RegisterEvent<TEventType>();
            UIManagerComponent.Instance.RegisterUIEvent(typeof(TEventType), window, this);
        }
    }
}
