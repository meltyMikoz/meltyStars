using KuusouEngine.EngineBasic.Entity;
using KuusouEngine.EngineBasic.Event;
using KuusouEngine.EngineImplement;
using KuusouEngine.EngineImplement.Entity;
using KuusouEngine.EngineImplement.Event;
using UnityEngine;

namespace GameMainTest.Entity
{
    public class CubeEntitySystem : ISystem<CubeEntityBehaviour>
    {
        protected override void OnActivate(CubeEntityBehaviour entity, object userData)
        {
            entity.gameObject.SetActive(true);
        }

        protected override void OnAttached(CubeEntityBehaviour entity, IEntity childEntity, object userData)
        {
            
        }

        protected override void OnAttachTo(CubeEntityBehaviour entity, IEntity parentEntity, object userData)
        {
            
        }

        protected override void OnDetached(CubeEntityBehaviour entity, IEntity childEntity, object userData)
        {
            
        }

        protected override void OnDetachFrom(CubeEntityBehaviour entity, IEntity parentEntity, object userData)
        {
            
        }

        protected override void OnInactivate(CubeEntityBehaviour entity, object userData)
        {
            entity.gameObject.SetActive(false);
            UnityEngine.Debug.Log(userData);
        }

        protected override void OnInit(int entityId, CubeEntityBehaviour entity, IEntityGroup entityGroup, IEntityInfo entityInfo, object userData)
        {
            Debug.Log("init");
            Entry.EventManager.Subcribe(new TestEventHandler());
        }

        protected override void OnRecycle(CubeEntityBehaviour entity, bool isShutDown)
        {
            
        }

        protected override void OnUpdate(CubeEntityBehaviour entity, float elapseFrequency, float elapseFrequencyReally)
        {
            if (entity.Time >= 5f)
            {
                entity.Inactivate(entity.Time);
                Entry.EventManager.Publish(this, new TestEvent() { id = 5555 });
            }
            entity.Time += elapseFrequency;
        }
    }
    public static class CubeEntitySystemExtension
    {
        public static void SetScale(this CubeEntityBehaviour self, Vector3 scale)
        {
            self.transform.localScale = scale;
        }
    }

    public struct TestEvent : IEvent
    {
        public int id;
    }

    public class TestEventHandler : AbstractEventHandler<TestEvent>
    {
        protected override void InternalHandle(object sender, TestEvent eventArgs)
        {
            Debug.Log(sender);
            Debug.Log(eventArgs.id);
        }
    }
}
