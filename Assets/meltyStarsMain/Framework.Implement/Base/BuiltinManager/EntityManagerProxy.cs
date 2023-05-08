using KuusouEngine.EngineBasic.Entity;
using KuusouEngine.EngineImplement.Entity;
using System;
using System.Collections.Generic;

namespace KuusouEngine.EngineImplement
{
    public class EntityManagerProxy : BuiltinManagerProxy<IEntityManager>
    {
        public int EntityCount
        {
            get
            {
                return _manager.EntityCount;
            }
        }

        public int EntityGroupCount
        {
            get
            {
                return _manager.EntityGroupCount;
            }
        }

        public void ActivateEntity(int entityId)
        {
            _manager.ActivateEntity(entityId);
        }

        public void ActivateEntity(int entityId, object userData)
        {
            _manager.ActivateEntity(entityId, userData);
        }

        public void ActivateEntity(IEntity entity)
        {
            _manager.ActivateEntity(entity);
        }

        public void ActivateEntity(IEntity entity, object userData)
        {
            _manager.ActivateEntity(entity, userData);
        }

        public bool AddEntityGroup(string entityGroupName, IEntityGroupHelper entityGroupHelper)
        {
            return _manager.AddEntityGroup(entityGroupName, entityGroupHelper);
        }

        public void AttachEntity(int childEntityId, int parentEntityId)
        {
            _manager.AttachEntity(childEntityId, parentEntityId);
        }

        public void AttachEntity(int childEntityId, int parentEntityId, object userData)
        {
            _manager.AttachEntity(childEntityId, parentEntityId, userData);
        }

        public void AttachEntity(IEntity childEntity, int parentEntityId)
        {
            _manager.AttachEntity(childEntity, parentEntityId);
        }

        public void AttachEntity(IEntity childEntity, int parentEntityId, object userData)
        {
            _manager.AttachEntity(childEntity, parentEntityId, userData);
        }

        public void AttachEntity(int childEntityId, IEntity parentEntity)
        {
            _manager.AttachEntity(childEntityId, parentEntity);
        }

        public void AttachEntity(int childEntityId, IEntity parentEntity, object userData)
        {
            _manager.AttachEntity(childEntityId, parentEntity, userData);
        }

        public void AttachEntity(IEntity childEntity, IEntity parentEntity)
        {
            _manager.AttachEntity(childEntity, parentEntity);
        }

        public void AttachEntity(IEntity childEntity, IEntity parentEntity, object userData)
        {
            _manager.AttachEntity(childEntity, parentEntity, userData);
        }

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entityId"></param>
        /// <param name="entityGroupName"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        public TEntity CreateEntity<TEntity>(int entityId, string entityGroupName, object userData = null) where TEntity : class, IEntity
        {
            return CreateEntity(typeof(TEntity), entityId, entityGroupName, userData) as TEntity;
        }

        public IEntity CreateEntity(Type entityType, int entityId, string entityGroupName, object userData = null)
        {
            return _manager.CreateEntity(entityType, entityId, entityGroupName, userData);
        }

        public IEntity CreateEntity(Type entityType, int entityId, IEntityGroup entityGroup, object userData = null)
        {
            return _manager.CreateEntity(entityType, entityId, entityGroup, userData);
        }

        public void DetachChildEntities(int parentEntityId)
        {
            _manager.DetachChildEntities(parentEntityId);
        }

        public void DetachChildEntities(int parentEntityId, object userData = null)
        {
            _manager.DetachChildEntities(parentEntityId, userData);
        }

        public void DetachChildEntities(IEntity parentEntity)
        {
            _manager.DetachChildEntities(parentEntity);
        }

        public void DetachChildEntities(IEntity parentEntity, object userData = null)
        {
            _manager.DetachChildEntities(parentEntity, userData);
        }

        public void DetachEntity(int childEntityId)
        {
            _manager.DetachEntity(childEntityId);
        }

        public void DetachEntity(int childEntityId, object userData = null)
        {
            _manager.DetachEntity(childEntityId, userData);
        }

        public void DetachEntity(IEntity childEntity)
        {
            _manager.DetachEntity(childEntity);
        }

        public void DetachEntity(IEntity childEntity, object userData = null)
        {
            _manager.DetachEntity(childEntity, userData);
        }

        public IEntityGroup[] GetAllEntityGroups()
        {
            return _manager.GetAllEntityGroups();
        }

        public void GetAllEntityGroups(List<IEntityGroup> results)
        {
            _manager.GetAllEntityGroups(results);
        }

        public IEntity[] GetChildEntities(int parentEntityId)
        {
            return _manager.GetChildEntities(parentEntityId);
        }

        public IEntity[] GetChildEntities(IEntity parentEntity)
        {
            return _manager.GetChildEntities(parentEntity);
        }

        public void GetChildEntities(int parentEntityId, List<IEntity> results)
        {
            _manager.GetChildEntities(parentEntityId, results);
        }

        public void GetChildEntities(IEntity parentEntity, List<IEntity> results)
        {
            _manager.GetChildEntities(parentEntity, results);
        }

        public int GetChildEntitiyCount(int parentEntityId)
        {
            return _manager.GetChildEntitiyCount(parentEntityId);
        }

        public IEntity GetChildEntity(int parentEntityId)
        {
            return _manager.GetEntity(parentEntityId);
        }

        public IEntity GetChildEntity(IEntity parentEntity)
        {
            return _manager.GetChildEntity(parentEntity);
        }

        public IEntityInfo GetEntityInfo(int entityId)
        {
            return _manager.GetEntityInfo(entityId);
        }

        public IEntity GetEntity(int entityId)
        {
            return _manager.GetEntity(entityId);
        }

        public IEntityGroup GetEntityGroup(string entityGroupName)
        {
            return _manager.GetEntityGroup(entityGroupName);
        }

        public IEntity GetParentEntity(int childEntityId)
        {
            return _manager.GetParentEntity(childEntityId);
        }

        public IEntity GetParentEntity(IEntity childEntity)
        {
            return _manager.GetParentEntity(childEntity);
        }

        public bool HasEntity(int entityId)
        {
            return _manager.HasEntity(entityId);
        }

        public bool HasEntityGroup(string entityGroupName)
        {
            return _manager.HasEntityGroup(entityGroupName);
        }

        public void InactivateEntity(int entityId)
        {
            _manager.InactivateEntity(entityId);
        }

        public void InactivateEntity(int entityId, object userData = null)
        {
            _manager.InactivateEntity(entityId, userData);
        }

        public void InactivateEntity(IEntity entity)
        {
            _manager.InactivateEntity(entity);
        }

        public void InactivateEntity(IEntity entity, object userData = null)
        {
            _manager.InactivateEntity(entity, userData);
        }

        public bool IsActived(int entityId)
        {
            return _manager.IsActived(entityId);
        }

        public bool IsActived(IEntity entity)
        {
            return _manager.IsActived(entity); 
        }

        public bool IsValid(IEntity entity)
        {
           return _manager.IsValid(entity);
        }

        public void RecycleEntity(int entityId)
        {
            _manager.RecycleEntity(entityId);
        }

        public void RecycleEntity(IEntity entity)
        {
            _manager.RecycleEntity(entity);
        }

        public void RegisterEntity(IEntity entity, string entityGroupName, object userData = null)
        {
            _manager.RegisterEntity(entity, entityGroupName, userData);
        }

        public void RegisterEntity(IEntity entity, IEntityGroup entityGroup, object userData = null)
        {
            _manager.RegisterEntity(entity, entityGroup, userData);
        }

        public void RemoveComponent<TComponent>(int entityId)
        {
            _manager.RemoveComponent<TComponent>(entityId);
        }

        public void RemoveComponent(int entityId, Type componentType)
        {
            _manager.RemoveComponent(entityId, componentType);
        }

        public void RemoveComponent<TComponent>(IEntity entity)
        {
            _manager.RemoveComponent<TComponent>(entity);
        }

        public void RemoveComponent(IEntity entity, Type componentType)
        {
            _manager.RemoveComponent(entity, componentType);
        }

        public TComponent AddComponent<TComponent>(int entityId) where TComponent : class, IComponent
        {
            return _manager.AddComponent<TComponent>(entityId);
        }

        public TComponent AddComponent<TComponent>(IEntity entity) where TComponent : class, IComponent
        {
            return _manager.AddComponent<TComponent>(entity);
        }

        public IComponent AddComponent(int entityId, Type componentType)
        {
            return _manager.AddComponent(entityId, componentType);
        }

        public IComponent AddComponent(IEntity entity, Type componentType)
        {
            return _manager.AddComponent(entity, componentType);
        }

        public TComponent GetComponent<TComponent>(int entityId) where TComponent : class, IComponent
        {
            return _manager.GetComponent<TComponent>(entityId);
        }

        public TComponent GetComponent<TComponent>(IEntity entity) where TComponent : class, IComponent
        {
            return _manager.GetComponent<TComponent>(entity);
        }

        public IComponent GetComponent(int entityId, Type componentType)
        {
            return _manager.GetComponent(entityId, componentType);
        }

        public IComponent GetComponent(IEntity entity, Type componentType)
        {
            return _manager.GetComponent(entity, componentType);
        }
    }
}
