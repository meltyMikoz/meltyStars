namespace KuusouEngine.EngineBasic.Entity
{
    internal sealed partial class EntityManager
    {
        private sealed class DefaultEntitySystem : ISystem
        {
            public static readonly DefaultEntitySystem Instance = new DefaultEntitySystem();
            public void OnActivate(IEntity entity, object userData)
            {
                
            }

            public void OnAttached(IEntity entity, IEntity childEntity, object userData)
            {
                
            }

            public void OnAttachTo(IEntity entity, IEntity parentEntity, object userData)
            {
                
            }

            public void OnDetached(IEntity entity, IEntity childEntity, object userData)
            {
                
            }

            public void OnDetachFrom(IEntity entity, IEntity parentEntity, object userData)
            {
                
            }

            public void OnInactivate(IEntity entity, object userData)
            {
                
            }

            public void OnInit(int entityId, IEntity entity, IEntityGroup entityGroup, IEntityInfo entityInfo, object userData)
            {
                entity.EntityGroup = entityGroup;
                entity.EntityInfo = entityInfo;
            }

            public void OnRecycle(IEntity entity, bool isShutDown)
            {
                
            }

            public void OnUpdate(IEntity entity, float elapseFrequency, float elapseFrequencyReally)
            {
                
            }
        }
    }
}
