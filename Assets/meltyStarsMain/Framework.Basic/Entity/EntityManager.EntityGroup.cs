using System;
using System.Collections.Generic;

namespace KuusouEngine.EngineBasic.Entity
{
    internal sealed partial class EntityManager
    {
        /// <summary>
        /// 实体组
        /// </summary>
        private sealed class EntityGroup : IEntityGroup
        {
            private string _name;
            private IEntityGroupHelper _helper;
            private readonly List<IEntity> _entities;
            public EntityGroup(string entityGroupName, IEntityGroupHelper entityGroupHelper)
            {
                this._name = entityGroupName;
                this._helper = entityGroupHelper;
                this._entities = new List<IEntity>();
                this._helper.CreateEntityGroup(entityGroupName);
            }
            /// <summary>
            /// 实体组名称
            /// </summary>
            /// <value></value>
            public string Name
            {
                get
                {
                    return this._name;
                }
            }
            /// <summary>
            /// 实体组实体数量
            /// </summary>
            /// <value></value>
            public int EntityCount
            {
                get
                {
                    return this._entities.Count;
                }
            }
            public IEntityGroupHelper Helper
            {
                get
                {
                    return this._helper;
                }
            }
            /// <summary>
            /// 实体组获取所有实体
            /// </summary>
            /// <returns></returns>
            public IEntity[] GetAllEntities()
            {
                return this._entities.ToArray();
            }
            /// <summary>
            /// 实体组获取所有实体
            /// </summary>
            /// <param name="results">目标容器</param>
            public void GetAllEntities(List<IEntity> results)
            {
                if (results is null)
                {
                    return;
                }
                results.Clear();
                foreach (IEntity entity in this._entities)
                {
                    results.Add(entity);
                }
            }
            /// <summary>
            /// 实体组获取实体集合
            /// </summary>
            /// <param name="inherit">是否获取继承类型</param>
            /// <typeparam name="T">实体泛型类型</typeparam>
            /// <returns></returns>
            public T[] GetEntities<T>(bool inherit) where T : IEntity
            {
                IEntity[] entities = GetEntities(typeof(T), inherit);
                T[] specificEntities = new T[entities.Length];
                if (entities.Length > 0)
                {
                    for (int i = 0; i < entities.Length; i++)
                    {
                        specificEntities[i] = (T)entities[i];
                    }
                }
                return specificEntities;
            }
            /// <summary>
            /// 实体组获取实体集合
            /// </summary>
            /// <param name="entityType">实体类型</param>
            /// <param name="inherit">是否获取继承类型</param>
            /// <returns></returns>
            public IEntity[] GetEntities(Type entityType, bool inherit)
            {
                List<IEntity> entities = new List<IEntity>();
                this._entities.ForEach(entity =>
                {
                    Type currentEntityType = entity.GetType();
                    bool match = currentEntityType == entityType;
                    if (!match && inherit)
                        match = currentEntityType.IsSubclassOf(entityType);
                    if (match)
                        entities.Add(entity);
                });
                return entities.ToArray();
            }
            /// <summary>
            /// 实体组获取实体
            /// </summary>
            /// <param name="entityId">实体Id</param>
            /// <returns></returns>
            public IEntity GetEntity(int entityId)
            {
                foreach (IEntity entity in this._entities)
                {
                    if (entity.Id == entityId)
                    {
                        return entity;
                    }
                }
                return null;
            }
            /// <summary>
            /// 实体组是否拥有实体
            /// </summary>
            /// <param name="entityId">实体Id</param>
            /// <returns></returns>
            public bool HasEntity(int entityId)
            {
                foreach (IEntity entity in this._entities)
                {
                    if (entity.Id == entityId)
                    {
                        return true;
                    }
                }
                return false;
            }
            /// <summary>
            /// 实体组添加实体
            /// </summary>
            /// <param name="entity">实体实例</param>
            public void AddEntity(IEntity entity)
            {
                if (entity is null)
                {
                    return;
                }
                this._entities.Add(entity);
                this._helper.AddEntity(this._name, entity);
            }
            /// <summary>
            /// 实体组移除实体
            /// </summary>
            /// <param name="entity">实体实例</param>
            public void RemoveEntity(IEntity entity)
            {
                if (entity is null)
                {
                    return;
                }
                if (!this._entities.Contains(entity))
                {
                    return;
                }
                this._entities.Remove(entity);
                this._helper.RemoveEntity(this._name, entity);
            }
            /// <summary>
            /// 实体组设置辅助器
            /// </summary>
            /// <param name="entityGroupHelper">实体辅助器</param>
            public void SetEntityGroupHelper(IEntityGroupHelper entityGroupHelper)
            {
                if (entityGroupHelper is null)
                {
                    return;
                }
                this._helper = entityGroupHelper;
            }
            public void Update(float elapseFrequency, float elapseFrequencyReally)
            {
                foreach (IEntity entity in this._entities)
                {
                    entity.EntityInfo.Update(elapseFrequency, elapseFrequencyReally);
                }
            }
        }
    }
}