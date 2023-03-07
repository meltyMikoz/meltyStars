using KuusouEngine.EngineBasic.Entity;

namespace KuusouEngine.EngineImplement.Entity
{
    public abstract class EntityGroupHelperBase : IEntityGroupHelper
    {
        public abstract void AddEntity(string entityGroupName, IEntity entity);

        public abstract void CreateEntityGroup(string entityGroupName);

        public abstract void RemoveEntity(string entityGroupName, IEntity entity);
    }
}
