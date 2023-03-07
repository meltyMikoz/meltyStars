using System;
using System.Collections.Generic;
using System.Reflection;

namespace KuusouEngine.EngineBasic.Entity
{
    internal sealed partial class EntityManager : KuusouEngineBasicModule, IEntityManager
    {
        private readonly Dictionary<int, EntityInfo> _entityInfos;
        private readonly Dictionary<string, EntityGroup> _entityGroups;
        private readonly Dictionary<Type, ISystem> _entitySystems;
        private bool _isShutDown;
        public EntityManager()
        {
            this._entityInfos = new Dictionary<int, EntityInfo>();
            this._entityGroups = new Dictionary<string, EntityGroup>();
            this._entitySystems = new Dictionary<Type, ISystem>();
            this._isShutDown = false;
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
            RecycleAllEntities();
            this._entityInfos.Clear();
            this._entityGroups.Clear();
            this._entitySystems.Clear();
        }
        private void RecycleAllEntities()
        {
            int[] entityIds = new int[this._entityInfos.Count];
            int index = 0;
            foreach (KeyValuePair<int, EntityInfo> entityInfo in this._entityInfos)
            {
                entityIds[index++] = entityInfo.Key;
            }
            foreach (int id in entityIds)
            {
                RecycleEntity(id);
            }
        }
        internal override void Update(float elapseFrequency, float elapseFrequencyReally)
        {
            foreach (KeyValuePair<string, EntityGroup> entityGroup in this._entityGroups)
            {
                entityGroup.Value.Update(elapseFrequency, elapseFrequencyReally);
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
            parentInfo.AddChildEntity(childEntity);
            parentInfo.OnAttached(childEntity, userData);
            childInfo.OnAttachTo(parentEntity, userData);
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

        public IEntity CreateEntity(Type entityType, int entityId, string entityGroupName, object userData)
        {
            if (HasEntity(entityId))
            {
                throw new KuusouEngineException("Entity Id is already used by an existing entity");
            }
            if (!HasEntityGroup(entityGroupName))
            {
                throw new KuusouEngineException("Entity group is not exist");
            }
            IEntityGroup entityGroup = GetEntityGroup(entityGroupName);
            return InternalCreateEntity(entityType, entityId, entityGroup, userData);
        }

        public IEntity CreateEntity(Type entityType, int entityId, IEntityGroup entityGroup, object userData)
        {
            if (HasEntity(entityId))
            {
                throw new KuusouEngineException("Entity Id is already used by an existing entity");
            }
            return InternalCreateEntity(entityType, entityId, entityGroup, userData);
        }

        private IEntity InternalCreateEntity(Type entityType, int entityId, IEntityGroup entityGroup, object userData)
        {
            IEntity entity = Activator.CreateInstance(entityType) as IEntity;
            EntityInfo entityInfo = EntityInfo.Create(entity);
            (entityGroup as EntityGroup).AddEntity(entity);
            entityInfo.Status = EntityStatus.Inited;
            this._entityInfos.Add(entityId, entityInfo);

            ISystem system = GetSystem(entityType);
            if (!(system is null))
            {
                entityInfo.SetSystem(system);
            }

            entityInfo.OnInit(entityGroup, userData);
            ActivateEntity(entity);
            return entity;
        }

        private ISystem GetSystem(Type entityType)
        {
            if (this._entitySystems.ContainsKey(entityType))
            {
                return this._entitySystems[entityType];
            }
            BindWithSystemAttribute bind = entityType.GetCustomAttribute(typeof(BindWithSystemAttribute), true) as BindWithSystemAttribute;
            if (!(bind is null))
            {
                Type systemType = bind.SystemType;
                if (!typeof(ISystem).IsAssignableFrom(systemType))
                {
                    throw new KuusouEngineException("System type is invalid");
                }
                ISystem system = Activator.CreateInstance(systemType) as ISystem;
                this._entitySystems.Add(entityType, system);
                return system;
            }
            return null;
        }

        public void RecycleEntity(int entityId)
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
            entityInfo.OnRecycle(this._isShutDown);
            EntityInfo.Release(entityInfo);
        }

        public void RecycleEntity(IEntity entity)
        {
            if (entity is null)
            {
                throw new KuusouEngineException("Entity is invalid");
            }
            RecycleEntity(entity.Id);
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
            parentInfo.RemoveChildEntity(childEntity);
            parentInfo.OnDetached(childEntity, userData);
            childInfo.OnDetachFrom(parentEntity, userData);
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

        public void InactivateEntity(int entityId)
        {
            InactivateEntity(entityId, null);
        }

        public void InactivateEntity(int entityId, object userData)
        {
            EntityInfo entityInfo = GetEntityInfo(entityId);
            if (entityInfo is null)
            {
                throw new KuusouEngineException("Entity is not registered");
            }
            entityInfo.Status &= ~EntityStatus.Actived;
            entityInfo.Status |= EntityStatus.Inactived;
            entityInfo.OnInactivate(userData);
        }

        public void InactivateEntity(IEntity entity)
        {
            if (entity is null)
            {
                throw new KuusouEngineException("Entity is invalid");
            }
            InactivateEntity(entity.Id, null);
        }

        public void InactivateEntity(IEntity entity, object userData)
        {
            if (entity is null)
            {
                throw new KuusouEngineException("Entity is invalid");
            }
            InactivateEntity(entity.Id, userData);
        }

        public void ActivateEntity(int entityId)
        {
            ActivateEntity(entityId, null);
        }

        public void ActivateEntity(int entityId, object userData)
        {
            EntityInfo entityInfo = GetEntityInfo(entityId);
            if (entityInfo is null)
            {
                throw new KuusouEngineException("Entity is not registered");
            }
            entityInfo.Status &= ~EntityStatus.Inactived;
            entityInfo.Status |= EntityStatus.Actived;
            entityInfo.OnActivate(userData);
        }

        public void ActivateEntity(IEntity entity)
        {
            if (entity is null)
            {
                throw new KuusouEngineException("Entity is not registered");
            }
            ActivateEntity(entity.Id, null);
        }

        public void ActivateEntity(IEntity entity, object userData)
        {
            if (entity is null)
            {
                throw new KuusouEngineException("Entity is not registered");
            }
            ActivateEntity(entity.Id, userData);
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

        public bool IsActived(int entityId)
        {
            EntityInfo entityInfo = GetEntityInfo(entityId);
            if (entityInfo is null)
            {
                throw new KuusouEngineException("Entity is not registered");
            }
            return (entityInfo.Status & EntityStatus.Actived) == EntityStatus.Actived;
        }

        public bool IsActived(IEntity entity)
        {
            return IsActived(entity.Id);
        }

        public bool IsValid(IEntity entity)
        {
            if (entity is null)
            {
                return false;
            }
            return HasEntity(entity.Id);
        }

        public void RegisterEntity(IEntity entity, string entityGroupName, object userData)
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
            InternalRegisterEntity(entity, entityGroup, userData);
        }

        public void RegisterEntity(IEntity entity, IEntityGroup entityGroup, object userData)
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
            InternalRegisterEntity(entity, entityGroup, userData);
        }
        private void InternalRegisterEntity(IEntity entity, IEntityGroup entityGroup, object userData)
        {
            EntityGroup group = entityGroup as EntityGroup;
            EntityInfo entityInfo = EntityInfo.Create(entity);
            entityInfo.Status = EntityStatus.Unknown;
            this._entityInfos.Add(entity.Id, entityInfo);
            group.AddEntity(entity);

            ISystem system = GetSystem(entity.GetType());
            if (!(system is null))
            {
                entityInfo.SetSystem(system);
            }
            entityInfo.OnInit(entityGroup, userData);
        }
        public TComponent AddComponent<TComponent>(int entityId) where TComponent : class, IComponent
        {
            return AddComponent(entityId, typeof(TComponent)) as TComponent;
        }

        public IComponent AddComponent(int entityId, Type componentType)
        {
            EntityInfo entityInfo= GetEntityInfo(entityId);
            if (entityInfo is null)
            {
                throw new KuusouEngineException("Target Entity is not registered");
            }
            return entityInfo.GetComponent(componentType);
        }

        public TComponent AddComponent<TComponent>(IEntity entity) where TComponent : class, IComponent
        {
            return AddComponent<TComponent>(entity.Id);
        }

        public IComponent AddComponent(IEntity entity, Type componentType)
        {
            return AddComponent(entity.Id, componentType);
        }

        public void RemoveComponent<TComponent>(int entityId)
        {
            RemoveComponent(entityId, typeof(TComponent));
        }

        public void RemoveComponent(int entityId, Type componentType)
        {
            EntityInfo entityInfo = GetEntityInfo(entityId);
            if (entityInfo is null)
            {
                throw new KuusouEngineException("Target Entity is not registered");
            }
            entityInfo.RemoveComponent(componentType);
        }

        public void RemoveComponent<TComponent>(IEntity entity)
        {
            RemoveComponent<TComponent>(entity.Id);
        }

        public void RemoveComponent(IEntity entity, Type componentType)
        {
            RemoveComponent(entity.Id, componentType);
        }

        public TComponent GetComponent<TComponent>(int entityId) where TComponent : class, IComponent
        {
            return GetComponent(entityId, typeof(TComponent)) as TComponent;
        }

        public IComponent GetComponent(int entityId, Type componentType)
        {
            EntityInfo entityInfo = GetEntityInfo(entityId);
            if (entityInfo is null)
            {
                throw new KuusouEngineException("Target Entity is not registered");
            }
            return entityInfo.GetComponent(componentType);
        }

        public TComponent GetComponent<TComponent>(IEntity entity) where TComponent : class, IComponent
        {
            return GetComponent<TComponent>(entity.Id);
        }

        public IComponent GetComponent(IEntity entity, Type componentType)
        {
            return GetComponent(entity.Id, componentType);
        }
    }
}
