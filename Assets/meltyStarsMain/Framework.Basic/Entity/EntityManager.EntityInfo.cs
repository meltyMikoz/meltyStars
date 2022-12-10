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
            private IEntityInfo _parentEntityInfo;
            private List<IEntity> _childEntities;
            private EntityStatus _status;
            private EntityUpdateMode _updateMode;
            public EntityInfo()
            {
                this._entity = null;
                this._parentEntity = null;
                this._childEntities = new List<IEntity>();
                this._status = EntityStatus.UnInited;
                this._updateMode = EntityUpdateMode.Normal;
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
            /// 实体父实体信息接口
            /// </summary>
            /// <value></value>
            public IEntityInfo ParentEntityInfo
            {
                get
                {
                    return this._parentEntityInfo;
                }
                set
                {
                    this._parentEntityInfo = value;
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
                    //抛出警告 已经存在
                    return;
                }
                this._childEntities.Add(childEntity);
            }
            public void RemoveChildEntity(IEntity childEntity)
            {
                if (!this._childEntities.Remove(childEntity))
                {
                    //抛出警告 不存在
                }
            }

            public void Update(float elapseFrequency, float elapseFrequencyReally)
            {
                if ((this._status & EntityStatus.Disabled) == EntityStatus.Disabled)
                {
                    return;
                }
                if (this._parentEntity != null)
                {
                    if (((this._parentEntityInfo as EntityInfo).Status & EntityStatus.Disabled) == EntityStatus.Disabled)
                    {
                        if (this._updateMode == EntityUpdateMode.Normal)
                        {
                            return;
                        }
                    }
                }
                this._entity.OnUpdate(elapseFrequency, elapseFrequencyReally);
            }

            public void Clear()
            {
                this._entity = null;
                this._status = EntityStatus.Unknown;
                this._parentEntity = null;
                this._parentEntityInfo = null;
                this._childEntities.Clear();
            }
        }
    }
}
