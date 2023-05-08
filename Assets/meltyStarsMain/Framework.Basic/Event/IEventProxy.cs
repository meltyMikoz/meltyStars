using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using static UnityEngine.UI.GridLayoutGroup;

namespace KuusouEngine.EngineBasic.Event
{
    /// <summary>
    /// 事件代理者接口
    /// </summary>
    public interface IEventProxy : IReference
    {
        IEventProxyOwner Owner 
        { 
            get; 
        }
        Type OwnerType
        {
            get;
        }
        IEventManager EventManager
        {
            get;
        }
        void Subscribe(IEventHandler eventHandler);
        void Unsubscribe(IEventHandler eventHandler);
        void UnsubscribeType(Type eventType);
        void UnsubscribeAll();
        void Publish(Type eventType, object sender, IEvent eventArgs);
    }
    /// <summary>
    /// 事件代理者泛型接口
    /// </summary>
    /// <typeparam name="TOwner">代理拥有者类型</typeparam>
    public interface IEventProxy<TOwner> : IEventProxy
    {
        public new TOwner Owner
        {
            get;
        }
    }
}
