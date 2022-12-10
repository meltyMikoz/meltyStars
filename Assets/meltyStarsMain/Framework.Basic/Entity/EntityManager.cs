using System;
using System.Collections.Generic;

namespace KuusouEngine.EngineBasic.Entity
{
    internal sealed partial class EntityManager : KuusouEngineBasicModule, IEntityManager
    {
        private readonly Dictionary<int, EntityInfo> _entityInfos;
        private readonly Dictionary<string, EntityGroup> _entityGroups;
        private bool _isShutDown;
        private int _serialNumber;
        public EntityManager()
        {
            this._entityInfos = new Dictionary<int, EntityInfo>();
            this._entityGroups = new Dictionary<string, EntityGroup>();
            this._isShutDown = false;
            this._serialNumber = 0;
        }
        public int EntityCount
        {
            get
            {
                return this._entityInfos.Count;
            }
        }
        public int EntityGroupCount
        {
            get
            {
                return this._entityGroups.Count;
            }
        }

        internal override void ShutDown()
        {
            this._isShutDown = true;
            this._entityInfos.Clear();
            this._entityGroups.Clear();
        }
        internal override void Update(float elapseFrequency, float elapseFrequencyReally)
        {
            foreach (KeyValuePair<int, EntityInfo> entityInfo in this._entityInfos)
            {
                entityInfo.Value.Update(elapseFrequency, elapseFrequencyReally);
            }
        }
        /// <summary>
        /// 获取实体信息
        /// </summary>
        /// <param name="entityId">实体Id</param>
        /// <returns>实体信息</returns>
        private EntityInfo GetEntityInfo(int entityId)
        {
            EntityInfo entityInfo = null;
            if (this._entityInfos.TryGetValue(entityId, out entityInfo))
            {
                return entityInfo;
            }
            return null;
        }
        public bool AddEntityGroup(string entityGroupName, IEntityGroupHelper entityGroupHelper)
        {
            if (string.IsNullOrEmpty(entityGroupName))
            {
                throw new KuusouEngineException("Entity group name is invalid");
            }
            if (entityGroupHelper is null)
            {
                throw new KuusouEngineException("Entity group helper is invalid");
            }
            if (HasEntityGroup(entityGroupName))
            {
                //log实体组已存在
                return false;
            }
            EntityGroup entityGroup = new EntityGroup(entityGroupName, entityGroupHelper);
            this._entityGroups.Add(entityGroupName, entityGroup);
            return true;
        }
        public void AttachEntity(int childEntityId, int parentEntityId)
        {
            AttachEntity(childEntityId, parentEntityId, null);
        }

        public void AttachEntity(int childEntityId, int parentEntityId, object userData)
        {
            if (childEntityId == parentEntityId)
            {
                throw new KuusouEngineException("Can not attach entity to the entity with a same id");
            }
            EntityInfo childInfo = GetEntityInfo(childEntityId);
            EntityInfo parentInfo = GetEntityInfo(parentEntityId);
            if (childInfo is null)
            {
                throw new KuusouEngineException("Child entity is not registered");
            }
            if (parentInfo is null)
            {
                throw new KuusouEngineException("Parent entity is not registered");
            }
            IEntity childEntity = childInfo.Entity;
            IEntity parentEntity = parentInfo.Entity;
            DetachEntity(childEntity.Id, userData);
            childInfo.ParentEntity = parentInfo.Entity;
            childInfo.ParentEntityInfo = parentInfo;
            parentInfo.AddChildEntity(childEntity);
            parentEntity.OnAttached(childEntity, userData);
            childEntity.OnAttachTo(parentEntity, userData);
        }

        public void AttachEntity(IEntity childEntity, int parentEntityId)
        {
            if (childEntity is null)
            {
                throw new KuusouEngineException("Child entity is invalid");
            }
            AttachEntity(childEntity.Id, parentEntityId, null);
        }

        public void AttachEntity(IEntity childEntity, int parentEntityId, object userData)
        {
            if (childEntity is null)
            {
                throw new KuusouEngineException("Child entity is invalid");
            }
            AttachEntity(childEntity.Id, parentEntityId, userData);
        }

        public void AttachEntity(int childEntityId, IEntity parentEntity)
        {
            if (parentEntity is null)
            {
                throw new KuusouEngineException("Parent entity is invalid");
            }
            AttachEntity(childEntityId, parentEntity, null);
        }

        public void AttachEntity(int childEntityId, IEntity parentEntity, object userData)
        {
            if (parentEntity is null)
            {
                throw new KuusouEngineException("Parent entity is invalid");
            }
            AttachEntity(childEntityId, parentEntity, userData);
        }

        public void AttachEntity(IEntity childEntity, IEntity parentEntity)
        {
            if (childEntity is null)
            {
                throw new KuusouEngineException("Child entity is invalid");
            }
            if (parentEntity is null)
            {
                throw new KuusouEngineException("Parent entity is invalid");
            }
            AttachEntity(childEntity, parentEntity, null);
        }

        public void AttachEntity(IEntity childEntity, IEntity parentEntity, object userData)
        {
            if (childEntity is null)
            {
                throw new KuusouEngineException("Child entity is invalid");
            }
            if (parentEntity is null)
            {
                throw new KuusouEngineException("Parent entity is invalid");
            }
            AttachEntity(childEntity, parentEntity, userData);
        }

        public IEntity CreateEntity(Type entityType, int entityId, string entityGroupName, IEntityHelper entityHelper, object userData)
        {
            if (HasEntity(entityId))
            {
                throw new KuusouEngineException("Entity Id is already used by an existing entity");
            }
            if (entityHelper is null)
            {
                throw new KuusouEngineException("Entity helper is invalid");
            }
            if (!HasEntityGroup(entityGroupName))
            {
                throw new KuusouEngineException("Entity group is not exist");
            }
            IEntityGroup entityGroup = GetEntityGroup(entityGroupName);
            return InternalCreateEntity(entityType, entityId, entityGroup, entityHelper, userData);
        }

        public IEntity CreateEntity(Type entityType, int entityId, IEntityGroup entityGroup, IEntityHelper entityHelper, object userData)
        {
            if (HasEntity(entityId))
            {
                throw new KuusouEngineException("Entity Id is already used by an existing entity");
            }
            if (entityHelper is null)
            {
                throw new KuusouEngineException("Entity helper is invalid");
            }
            return InternalCreateEntity(entityType, entityId, entityGroup, entityHelper, userData);
        }

        private IEntity InternalCreateEntity(Type entityType, int entityId, IEntityGroup entityGroup, IEntityHelper entityHelper, object userData)
        {
            IEntity entity = Activator.CreateInstance(entityType) as IEntity;
            EntityInfo entityInfo = EntityInfo.Create(entity);
            (entityGroup as EntityGroup).AddEntity(entity);
            entity.OnInit(entityId, entityHelper, userData);
            entityInfo.Status = EntityStatus.Inited;
            this._entityInfos.Add(entityId, entityInfo);
            EnableEntity(entity);
            return entity;
        }

        public void DestroyEntity(int entityId)
        {
            EntityInfo entityInfo = GetEntityInfo(entityId);
            if (entityInfo is null)
            {
                throw new KuusouEngineException("Entity is not registered");
            }
            IEntity entity = entityInfo.Entity;
            EntityGroup entityGroup = entity.EntityGroup as EntityGroup;
            DetachChildEntities(entity);
            DetachEntity(entity);
            entityInfo.Status = EntityStatus.Destroyed;
            entityGroup.RemoveEntity(entity);
            this._entityInfos.Remove(entityId);
            entity.OnDestroy(this._isShutDown);
            EntityInfo.Release(entityInfo);
        }

        public void DestroyEntity(IEntity entity)
        {
            if (entity is null)
            {
                throw new KuusouEngineException("Entity is invalid");
            }
            DestroyEntity(entity.Id);
        }

        public void DetachChildEntities(int parentEntityId)
        {
            DetachChildEntities(parentEntityId, null);
        }

        public void DetachChildEntities(int parentEntityId, object userData)
        {
            EntityInfo parentEntityInfo = GetEntityInfo(parentEntityId);
            if (parentEntityInfo == null)
            {
                throw new KuusouEngineException("Parent entity is not registered");
            }
            while (parentEntityInfo.ChildEntityCount > 0)
            {
                IEntity childEntity = parentEntityInfo.GetChildEntity();
                DetachEntity(childEntity.Id, userData);
            }
        }

        public void DetachChildEntities(IEntity parentEntity)
        {
            if (parentEntity == null)
            {
                throw new KuusouEngineException("Parent entity is invalid.");
            }
            DetachChildEntities(parentEntity.Id, null);
        }

        public void DetachChildEntities(IEntity parentEntity, object userData)
        {
            if (parentEntity == null)
            {
                throw new KuusouEngineException("Parent entity is invalid.");
            }
            DetachChildEntities(parentEntity.Id, userData);
        }

        public void DetachEntity(int childEntityId)
        {
            DetachEntity(childEntityId, null);
        }

        public void DetachEntity(int childEntityId, object userData)
        {
            EntityInfo childInfo = GetEntityInfo(childEntityId);
            if (childInfo is null)
            {
                throw new KuusouEngineException("Child entity is not registered");
            }
            IEntity parentEntity = childInfo.ParentEntity;
            if (parentEntity is null)
            {
                return;
            }
            EntityInfo parentInfo = GetEntityInfo(parentEntity.Id);
            if (parentInfo is null)
            {
                throw new KuusouEngineException("Parent entity is not registered");
            }
            IEntity childEntity = childInfo.Entity;
            childInfo.ParentEntity = null;
            childInfo.ParentEntityInfo = null;
            parentInfo.RemoveChildEntity(childEntity);
            parentEntity.OnDetached(childEntity, userData);
            childEntity.OnDetachFrom(parentEntity, userData);
        }

        public void DetachEntity(IEntity childEntity)
        {
            if (childEntity is null)
            {
                throw new KuusouEngineException("Child entity is invalid");
            }
            DetachEntity(childEntity.Id, null);
        }

        public void DetachEntity(IEntity childEntity, object userData)
        {
            if (childEntity is null)
            {
                throw new KuusouEngineException("Child entity is invalid");
            }
            DetachEntity(childEntity.Id, userData);
        }

        public void DisableEntity(int entityId)
        {
            DisableEntity(entityId, null);
        }

        public void DisableEntity(int entityId, object userData)
        {
            EntityInfo entityInfo = GetEntityInfo(entityId);
            if (entityInfo is null)
            {
                throw new KuusouEngineException("Entity is not registered");
            }
            entityInfo.Status &= ~EntityStatus.Enabled;
            entityInfo.Status |= EntityStatus.Disabled;
            entityInfo.Entity.OnDisable(userData);
        }

        public void DisableEntity(IEntity entity)
        {
            if (entity is null)
            {
                throw new KuusouEngineException("Entity is invalid");
            }
            DisableEntity(entity.Id, null);
        }

        public void DisableEntity(IEntity entity, object userData)
        {
            if (entity is null)
            {
                throw new KuusouEngineException("Entity is invalid");
            }
            DisableEntity(entity.Id, userData);
        }

        public void EnableEntity(int entityId)
        {
            EnableEntity(entityId, null);
        }

        public void EnableEntity(int entityId, object userData)
        {
            EntityInfo entityInfo = GetEntityInfo(entityId);
            if (entityInfo is null)
            {
                throw new KuusouEngineException("Entity is not registered");
            }
            entityInfo.Status &= ~EntityStatus.Disabled;
            entityInfo.Status |= EntityStatus.Enabled;
            entityInfo.Entity.OnEnable(userData);
        }

        public void EnableEntity(IEntity entity)
        {
            if (entity is null)
            {
                throw new KuusouEngineException("Entity is not registered");
            }
            EnableEntity(entity.Id, null);
        }

        public void EnableEntity(IEntity entity, object userData)
        {
            if (entity is null)
            {
                throw new KuusouEngineException("Entity is not registered");
            }
            EnableEntity(entity.Id, userData);
        }

        public IEntityGroup[] GetAllEntityGroups()
        {
            int index = 0;
            IEntityGroup[] results = new IEntityGroup[this._entityGroups.Count];
            foreach (KeyValuePair<string, EntityGroup> entityGroup in this._entityGroups)
            {
                results[index++] = entityGroup.Value;
            }
            return results;
        }

        public void GetAllEntityGroups(List<IEntityGroup> results)
        {
            if (results == null)
            {
                throw new KuusouEngineException("Results is invalid");
            }
            results.Clear();
            foreach (KeyValuePair<string, EntityGroup> entityGroup in this._entityGroups)
            {
                results.Add(entityGroup.Value);
            }
        }

        public IEntity[] GetChildEntities(int parentEntityId)
        {
            EntityInfo parentEntityInfo = GetEntityInfo(parentEntityId);
            if (parentEntityInfo == null)
            {
                throw new KuusouEngineException("Parent entity is not registered");
            }
            return parentEntityInfo.GetChildEntities();
        }

        public IEntity[] GetChildEntities(IEntity parentEntity)
        {
            if (parentEntity == null)
            {
                throw new KuusouEngineException("Parent entity is invalid");
            }
            return GetChildEntities(parentEntity.Id);
        }

        public void GetChildEntities(int parentEntityId, List<IEntity> results)
        {
            EntityInfo parentEntityInfo = GetEntityInfo(parentEntityId);
            if (parentEntityInfo == null)
            {
                throw new KuusouEngineException("Parent entity is not registered");
            }
            parentEntityInfo.GetChildEntities(results);
        }

        public void GetChildEntities(IEntity parentEntity, List<IEntity> results)
        {
            if (parentEntity == null)
            {
                throw new KuusouEngineException("Parent entity is invalid");
            }
            GetChildEntities(parentEntity.Id, results);
        }

        public int GetChildEntitiyCount(int parentEntityId)
        {
            EntityInfo parentEntityInfo = GetEntityInfo(parentEntityId);
            if (parentEntityInfo == null)
            {
                throw new KuusouEngineException("Parent entity is not registered");
            }
            return parentEntityInfo.ChildEntityCount;
        }

        public IEntity GetChildEntity(int parentEntityId)
        {
            EntityInfo parentEntityInfo = GetEntityInfo(parentEntityId);
            if (parentEntityInfo == null)
            {
                throw new KuusouEngineException("Parent entity is not registered");
            }
            return parentEntityInfo.GetChildEntity();
        }

        public IEntity GetChildEntity(IEntity parentEntity)
        {
            if (parentEntity == null)
            {
                throw new KuusouEngineException("Parent entity is invalid");
            }
            return GetChildEntity(parentEntity.Id);
        }

        public IEntity GetEntity(int entityId)
        {
            if (!HasEntity(entityId))
            {
                return null;
            }
            return this._entityInfos[entityId].Entity;
        }

        public IEntityGroup GetEntityGroup(string entityGroupName)
        {
            if (string.IsNullOrEmpty(entityGroupName))
            {
                throw new KuusouEngineException("Entity group name is invalid");
            }
            if (!HasEntityGroup(entityGroupName))
            {
                return null;
            }
            return this._entityGroups[entityGroupName];
        }

        public IEntity GetParentEntity(int childEntityId)
        {
            EntityInfo childEntityInfo = GetEntityInfo(childEntityId);
            if (childEntityInfo is null)
            {
                throw new KuusouEngineException("Child entity is not registered");
            }
            return childEntityInfo.ParentEntity;
        }

        public IEntity GetParentEntity(IEntity childEntity)
        {
            if (childEntity is null)
            {
                throw new KuusouEngineException("Child entity is invalid");
            }
            return GetParentEntity(childEntity.Id);
        }

        public bool HasEntity(int entityId)
        {
            return this._entityInfos.ContainsKey(entityId);
        }

        public bool HasEntityGroup(string entityGroupName)
        {
            if (string.IsNullOrEmpty(entityGroupName))
            {
                throw new KuusouEngineException("Entity group name is invalid");
            }
            return this._entityGroups.ContainsKey(entityGroupName);
        }

        public bool IsEnabled(int entityId)
        {
            EntityInfo entityInfo = GetEntityInfo(entityId);
            if (entityInfo is null)
            {
                throw new KuusouEngineException("Entity is not registered");
            }
            return (entityInfo.Status & EntityStatus.Enabled) == EntityStatus.Enabled;
        }

        public bool IsEnabled(IEntity entity)
        {
            return IsEnabled(entity.Id);
        }

        public bool IsValid(IEntity entity)
        {
            if (entity is null)
            {
                return false;
            }
            return HasEntity(entity.Id);
        }

        public void RegisterEntity(IEntity entity, string entityGroupName)
        {
            if (entity is null)
            {
                throw new KuusouEngineException("Entity is invalid");
            }
            if (HasEntity(entity.Id))
            {
                throw new KuusouEngineException("Entity Id is already used by an existing entity");
            }
            if (string.IsNullOrEmpty(entityGroupName))
            {
                throw new KuusouEngineException("Entity group name is invalid");
            }
            if (!HasEntityGroup(entityGroupName))
            {
                throw new KuusouEngineException("Entity group is not exist");
            }
            EntityGroup entityGroup = GetEntityGroup(entityGroupName) as EntityGroup;
        }

        public void RegisterEntity(IEntity entity, IEntityGroup entityGroup)
        {
            if (entity is null)
            {
                throw new KuusouEngineException("Entity is invalid");
            }
            if (entityGroup is null)
            {
                throw new KuusouEngineException("Entity group is invalid");
            }
            if (HasEntity(entity.Id))
            {
                throw new KuusouEngineException("Entity Id is already used by an existing entity");
            }
            InternalRegisterEntity(entity, entityGroup);
        }
        private void InternalRegisterEntity(IEntity entity, IEntityGroup entityGroup)
        {
            EntityGroup group = entityGroup as EntityGroup;
            EntityInfo entityInfo = EntityInfo.Create(entity);
            entityInfo.Status = EntityStatus.Unknown;
            this._entityInfos.Add(entity.Id, entityInfo);
            group.AddEntity(entity);
        }
    }
}
