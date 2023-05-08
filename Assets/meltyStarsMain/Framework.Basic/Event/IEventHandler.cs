using KuusouEngine.EngineBasic.Entity;
using System;

namespace KuusouEngine.EngineBasic.Event
{
    public interface IEventHandler : IReference
    {
        Type EventType { get; }

        void OnSubscribe(IEventProxyOwner owner);

        void OnUnsubscribe(IEventProxyOwner owner);

        void Handle(object sender, IEvent eventArgs);
    }
}
