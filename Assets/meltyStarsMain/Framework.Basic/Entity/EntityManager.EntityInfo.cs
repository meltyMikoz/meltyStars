using KuusouEngine.EngineImplement.Entity;
using System;
using System.Collections.Generic;

namespace KuusouEngine.EngineBasic.Entity
{
    internal sealed partial class EntityManager
    {
        /// <summary>
        /// 实体信息
        /// </summary>
        private sealed class EntityInfo : IEntityInfo, IReference
        {
            private IEntity _entity;
            private IEntity _parentEntity;
            private List<IEntity> _childEntities;
            private EntityStatus _status;
            private EntityUpdateMode _updateMode;
            private List<IComponent> _components;
            private ISystem _system;
            public EntityInfo()
            {
                this._entity = null;
                this._parentEntity = null;
                this._childEntities = new List<IEntity>();
                this._status = EntityStatus.UnInited;
                this._updateMode = EntityUpdateMode.Normal;
                this._components = new List<IComponent>();
            }
            /// <summary>
            /// 实体
            /// </summary>
            /// <value></value>
            public IEntity Entity
            {
                get
                {
                    return this._entity;
                }
            }
            /// <summary>
            /// 实体父实体
            /// </summary>
            /// <value></value>
            public IEntity ParentEntity
            {
                get
                {
                    return this._parentEntity;
                }
                set
                {
                    this._parentEntity = value;
                }
            }
            /// <summary>
            /// 实体状态
            /// </summary>
            /// <value></value>
            public EntityStatus Status
            {
                get
                {
                    return this._status;
                }
                set
                {
                    this._status = value;
                }
            }
            /// <summary>
            /// 实体轮询模式
            /// </summary>
            /// <value></value>
            public EntityUpdateMode UpdateMode
            {
                get
                {
                    return this._updateMode;
                }
                set
                {
                    this._updateMode = value;
                }
            }
            /// <summary>
            /// 实体组件集合
            /// </summary>
            public IComponent[] Components
            {
                get
                {
                    return this._components.ToArray();
                }
            }
            /// <summary>
            /// 实体子实体数量
            /// </summary>
            /// <value></value>
            public int ChildEntityCount
            {
                get
                {
                    return this._childEntities.Count;
                }
            }

            public static EntityInfo Create(IEntity entity)
            {
                if (entity is null)
                {
                    return null;
                }
                EntityInfo entityInfo = ReferencePool.Fetch<EntityInfo>();
                entityInfo._entity = entity;
                entityInfo._status = EntityStatus.UnInited;
                entityInfo._system = DefaultEntitySystem.Instance;
                return entityInfo;
            }
            public static void Release(EntityInfo entityInfo)
            {
                if (entityInfo is null)
                {
                    throw new KuusouEngineException("Entity info is invalid");
                }
                ReferencePool.Release(entityInfo);
            }
            public IEntity GetChildEntity()
            {
                return this._childEntities.Count > 0 ? this._childEntities[0] : null;
            }
            public IEntity[] GetChildEntities()
            {
                return this._childEntities.ToArray();
            }
            public void GetChildEntities(List<IEntity> results)
            {
                if (results is null)
                {
                    return;
                }
                results.Clear();
                foreach (IEntity childEntity in this._childEntities)
                {
                    results.Add(childEntity);
                }
            }
            public void AddChildEntity(IEntity childEntity)
            {
                if (this._childEntities.Contains(childEntity))
                {
                    throw new KuusouEngineException("child is already exist");
                }
                this._childEntities.Add(childEntity);
            }
            public void RemoveChildEntity(IEntity childEntity)
            {
                if (!this._childEntities.Remove(childEntity))
                {
                    throw new KuusouEngineException("child is not exist");
                }
            }
            public IComponent AddComponent(Type componentType)
            {
                if (!typeof(IComponent).IsAssignableFrom(componentType))
                {
                    throw new KuusouEngineException("AddComponent(Type) must be a derived class of IComponent");
                }
                return InternalAddComponent(componentType);
            }
            private IComponent InternalAddComponent(Type componentType)
            {
                Type type = null;
                foreach (IComponent component in this._components)
                {
                    type = component.GetType();
                    if (type == componentType)
                    {
                        throw new KuusouEngineException($"You are trying to add a component to an entity, but the entity already has a component of this type {componentType}");
                    }
                }
                IComponent newComponent = ReferencePool.Fetch(componentType) as IComponent;
                this._components.Add(newComponent);
                return newComponent;
            }
            public void RemoveComponent(Type componentType) 
            {
                if (!typeof(IComponent).IsAssignableFrom(componentType))
                {
                    throw new KuusouEngineException("RemoveComponent(Type) must be a derived class of IComponent");
                }
                InternalRemoveComponent(componentType);
            }
            private void InternalRemoveComponent(Type componentType) 
            {
                IComponent targetComponent = InternalGetComponent(componentType);
                if (targetComponent is null)
                {
                    throw new KuusouEngineException($"You are trying to remove a component from an entity, but the entity does not have a component of this type {componentType}");
                }
                this._components.Remove(targetComponent);
                ReferencePool.Release(targetComponent);
            }
            public IComponent GetComponent(Type componentType)
            {
                if (!typeof(IComponent).IsAssignableFrom(componentType))
                {
                    throw new KuusouEngineException("GetComponent(Type) must be a derived class of IComponent");
                }
                return InternalGetComponent(componentType);
            }
            private IComponent InternalGetComponent(Type componentType)
            {
                Type type = null;
                IComponent targetComponent = null;
                foreach (IComponent component in this._components)
                {
                    type = component.GetType();
                    if (type == componentType)
                    {
                        targetComponent = component;
                    }
                }
                return targetComponent;
            }

            public void SetSystem(ISystem system)
            {
                this._system = system;
            }

            public void OnInit(IEntityGroup entityGroup, object userData)
            {
                this._system.OnInit(this._entity.Id, this._entity, entityGroup, this, userData);
            }

            public void OnRecycle(bool isShutDown)
            {
                this._system.OnRecycle(this._entity, isShutDown);
            }

            public void OnActivate(object userData)
            {
                this._system.OnActivate(this._entity, userData);
            }

            public void OnInactivate(object userData)
            {
                this._system.OnInactivate(this._entity, userData);
            }

            public void OnAttached(IEntity childEntity, object userData)
            {
                this._system.OnAttached(this._entity, childEntity, userData);
            }

            public void OnDetached(IEntity childEntity, object userData)
            {
                this._system.OnDetached(this._entity, childEntity, userData);
            }

            public void OnAttachTo(IEntity parentEntity, object userData)
            {
                this._system.OnAttachTo(this._entity, parentEntity, userData);
            }

            public void OnDetachFrom(IEntity parentEntity, object userData)
            {
                this._system.OnDetachFrom(this._entity, parentEntity, userData);
            }

            public void OnUpdate(float elapseFrequency, float elapseFrequencyReally)
            {
                this._system.OnUpdate(this._entity, elapseFrequency, elapseFrequencyReally);
            }

            public void Update(float elapseFrequency, float elapseFrequencyReally)
            {
                if ((this._status & EntityStatus.Inactived) == EntityStatus.Inactived)
                {
                    return;
                }
                if (this._parentEntity != null)
                {
                    if (((this._parentEntity.EntityInfo as EntityInfo).Status & EntityStatus.Inactived) == EntityStatus.Inactived)
                    {
                        if (this._updateMode == EntityUpdateMode.Normal)
                        {
                            return;
                        }
                    }
                }
                this._system.OnUpdate(this._entity, elapseFrequency, elapseFrequencyReally);
            }

            public void Clear()
            {
                this._entity = null;
                this._status = EntityStatus.Unknown;
                this._parentEntity = null;
                this._childEntities.Clear();
            }
        }
    }
}
