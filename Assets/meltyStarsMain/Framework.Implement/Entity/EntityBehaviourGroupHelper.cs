using KuusouEngine.EngineBasic.Entity;
using UnityEngine;

namespace KuusouEngine.EngineImplement.Entity
{
    public class EntityBehaviourGroupHelper : EntityGroupHelperBase
    {
        public static readonly EntityGroupHelperBase Instance = new EntityBehaviourGroupHelper();
        private Transform _root;
        public override void AddEntity(string entityGroupName, IEntity entity)
        {
            Transform rootTransform = _root.Find(entityGroupName);
            EntityBehaviour entityBehaviour = entity as EntityBehaviour;
            entityBehaviour.transform.SetParent(rootTransform, true);
        }

        public override void CreateEntityGroup(string entityGroupName)
        {
            if (!this._root)
            {
                this._root = new GameObject("EntityGroupRoot").transform;
            }
            Transform transform = new GameObject(entityGroupName).transform;
            transform.SetParent(this._root, true);
        }

        public override void RemoveEntity(string entityGroupName, IEntity entity)
        {
            Transform rootTransform = _root.Find(entityGroupName);
            EntityBehaviour entityBehaviour = entity as EntityBehaviour;
            entityBehaviour.transform.SetParent(null, true);
        }
    }
}
