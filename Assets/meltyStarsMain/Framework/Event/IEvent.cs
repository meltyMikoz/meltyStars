using System;
using UnityEngine;

namespace MeltyStars
{
    public interface IEventHandler
    {
        Type GetTypeOfEvent();
    }

    [Event]
    public abstract class AEventHandler<TEventType> : IEventHandler where TEventType : struct
    {
        public Type GetTypeOfEvent()
        {
            return typeof(TEventType);
        }
        protected abstract void Execute(TEventType eventType);
        public void Handle(TEventType eventType)
        {
            try
            {
                Execute(eventType);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
    /// <summary>
    /// 使用Action的Handler
    /// </summary>
    /// <typeparam name="TEventType"></typeparam>
    public sealed class EventActionHandler<TEventType> : AEventHandler<TEventType> where TEventType : struct
    {
        public Action<TEventType> Action;
        protected override void Execute(TEventType eventType) { Action?.Invoke(eventType); }
    }
}
