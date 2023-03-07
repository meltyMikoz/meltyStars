using System;
using System.Collections.Generic;
namespace KuusouEngine.EngineBasic.Entity
{
    /// <summary>
    /// 实体管理器接口
    /// </summary>
    public interface IEntityManager
    {
        /// <summary>
        /// 实体数量
        /// </summary>
        /// <value></value>
        int EntityCount
        {
            get;
        }
        /// <summary>
        /// 实体组数量
        /// </summary>
        /// <value></value>
        int EntityGroupCount
        {
            get;
        }
        /// <summary>
        /// 实体是否存在
        /// </summary>
        /// <param name="entityId">实体Id</param>
        /// <returns>是否存在</returns>
        bool HasEntity(int entityId);
        /// <summary>
        /// 实体是否合法
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>是否合法</returns>
        bool IsValid(IEntity entity);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="entityId">实体Id</param>
        /// <returns>实体</returns>
        IEntity GetEntity(int entityId);
        /// <summary>
        /// 获取父实体
        /// </summary>
        /// <param name="childEntityId">子实体Id</param>
        /// <returns>父实体</returns>
        IEntity GetParentEntity(int childEntityId);
        /// <summary>
        /// 获取父实体
        /// </summary>
        /// <param name="childEntity">子实体</param>
        /// <returns>父实体</returns>
        IEntity GetParentEntity(IEntity childEntity);
        /// <summary>
        /// 获取子实体
        /// </summary>
        /// <param name="parentEntityId">父实体Id</param>
        /// <returns>首个子实体</returns>
        IEntity GetChildEntity(int parentEntityId);
        /// <summary>
        /// 获取子实体
        /// </summary>
        /// <param name="parentEntity">父实体</param>
        /// <returns>首个子实体</returns>
        IEntity GetChildEntity(IEntity parentEntity);
        /// <summary>
        /// 获取所有子实体
        /// </summary>
        /// <param name="parentEntityId">父实体Id</param>
        /// <returns>子实体集合</returns>
        IEntity[] GetChildEntities(int parentEntityId);
        /// <summary>
        /// 获取所有子实体
        /// </summary>
        /// <param name="parentEntity">父实体</param>
        /// <returns>子实体集合</returns>
        IEntity[] GetChildEntities(IEntity parentEntity);
        /// <summary>
        /// 获取所有子实体
        /// </summary>
        /// <param name="parentEntityId">父实体Id</param>
        /// <param name="results">目标容器</param>
        void GetChildEntities(int parentEntityId, List<IEntity> results);
        /// <summary>
        /// 获取所有子实体
        /// </summary>
        /// <param name="parentEntity">父实体</param>
        /// <param name="results">目标容器</param>
        void GetChildEntities(IEntity parentEntity, List<IEntity> results);
        /// <summary>
        /// 获取子实体数量
        /// </summary>
        /// <param name="parentEntityId">父实体Id</param>
        /// <returns>子实体数量</returns>
        int GetChildEntitiyCount(int parentEntityId);
        /// <summary>
        /// 实体组是否存在
        /// </summary>
        /// <param name="entityGroupName">实体组名称</param>
        /// <returns>是否存在</returns>
        bool HasEntityGroup(string entityGroupName);
        /// <summary>
        /// 获取实体组
        /// </summary>
        /// <param name="entityGroupName">实体组名称</param>
        /// <returns>实体组</returns>
        IEntityGroup GetEntityGroup(string entityGroupName);
        /// <summary>
        /// 获取所有实体组
        /// </summary>
        /// <returns>实体组集合</returns>
        IEntityGroup[] GetAllEntityGroups();
        /// <summary>
        /// 获取所有实体组
        /// </summary>
        /// <param name="results">目标容器</param>
        void GetAllEntityGroups(List<IEntityGroup> results);
        /// <summary>
        /// 增加实体组
        /// </summary>
        /// <param name="entityGroupName">实体组名称</param>
        /// <param name="entityGroupHelper">实体组辅助器</param>
        /// <returns>是否添加成功</returns>
        bool AddEntityGroup(string entityGroupName, IEntityGroupHelper entityGroupHelper);
        /// <summary>
        /// 附加子实体
        /// </summary>
        /// <param name="childEntityId">子实体Id</param>
        /// <param name="parentEntityId">父实体Id</param>
        void AttachEntity(int childEntityId, int parentEntityId);
        /// <summary>
        /// 附加子实体
        /// </summary>
        /// <param name="childEntityId">子实体Id</param>
        /// <param name="parentEntityId">父实体Id</param>
        /// <param name="userData">用户自定义数据</param>
        void AttachEntity(int childEntityId, int parentEntityId, object userData);
        /// <summary>
        /// 附加子实体
        /// </summary>
        /// <param name="childEntity">子实体</param>
        /// <param name="parentEntityId">父实体Id</param>
        void AttachEntity(IEntity childEntity, int parentEntityId);
        /// <summary>
        /// 附加子实体
        /// </summary>
        /// <param name="childEntity">子实体</param>
        /// <param name="parentEntityId">父实体Id</param>
        /// <param name="userData">用户自定义数据</param>
        void AttachEntity(IEntity childEntity, int parentEntityId, object userData);
        /// <summary>
        /// 附加子实体
        /// </summary>
        /// <param name="childEntityId">子实体Id</param>
        /// <param name="parentEntity">父实体</param>
        void AttachEntity(int childEntityId, IEntity parentEntity);
        /// <summary>
        /// 附加子实体
        /// </summary>
        /// <param name="childEntityId">子实体Id</param>
        /// <param name="parentEntity">父实体</param>
        /// <param name="userData">用户自定义数据</param>
        void AttachEntity(int childEntityId, IEntity parentEntity, object userData);
        /// <summary>
        /// 附加子实体
        /// </summary>
        /// <param name="childEntity">子实体</param>
        /// <param name="parentEntity">父实体</param>
        void AttachEntity(IEntity childEntity, IEntity parentEntity);
        /// <summary>
        /// 附加子实体
        /// </summary>
        /// <param name="childEntity">子实体</param>
        /// <param name="parentEntity">父实体</param>
        /// <param name="userData">用户自定义数据</param>
        void AttachEntity(IEntity childEntity, IEntity parentEntity, object userData);
        /// <summary>
        /// 解除子实体
        /// </summary>
        /// <param name="childEntityId">子实体Id</param>
        void DetachEntity(int childEntityId);
        /// <summary>
        /// 解除子实体
        /// </summary>
        /// <param name="childEntityId">子实体Id</param>
        /// <param name="userData">用户自定义数据</param>
        void DetachEntity(int childEntityId, object userData);
        /// <summary>
        /// 解除子实体
        /// </summary>
        /// <param name="childEntity">子实体</param>
        void DetachEntity(IEntity childEntity);
        /// <summary>
        /// 解除子实体
        /// </summary>
        /// <param name="childEntity">子实体</param>
        /// <param name="userData">用户自定义数据</param>
        void DetachEntity(IEntity childEntity, object userData);
        /// <summary>
        /// 解除所有子实体
        /// </summary>
        /// <param name="parentEntityId">父实体Id</param>
        void DetachChildEntities(int parentEntityId);
        /// <summary>
        /// 解除所有子实体
        /// </summary>
        /// <param name="parentEntityId">父实体Id</param>
        /// <param name="userData">用户自定义数据</param>
        void DetachChildEntities(int parentEntityId, object userData);
        /// <summary>
        /// 解除所有子实体
        /// </summary>
        /// <param name="parentEntity">父实体</param>
        void DetachChildEntities(IEntity parentEntity);
        /// <summary>
        /// 解除所有子实体
        /// </summary>
        /// <param name="parentEntity">父实体</param>
        /// <param name="userData">用户自定义数据</param>
        void DetachChildEntities(IEntity parentEntity, object userData);
        /// <summary>
        /// 激活实体
        /// </summary>
        /// <param name="entityId">实体Id</param>
        void ActivateEntity(int entityId);
        /// <summary>
        /// 激活实体
        /// </summary>
        /// <param name="entityId">实体Id</param>
        /// <param name="userData">用户自定义数据</param>
        void ActivateEntity(int entityId, object userData);
        /// <summary>
        /// 激活实体
        /// </summary>
        /// <param name="entity">实体</param>
        void ActivateEntity(IEntity entity);
        /// <summary>
        /// 激活实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="userData">用户自定义数据</param>
        void ActivateEntity(IEntity entity, object userData);
        /// <summary>
        /// 失活实体
        /// </summary>
        /// <param name="entityId">实体Id</param>
        void InactivateEntity(int entityId);
        /// <summary>
        /// 失活实体
        /// </summary>
        /// <param name="entityId">实体Id</param>
        /// <param name="userData">用户自定义数据</param>
        void InactivateEntity(int entityId, object userData);
        /// <summary>
        /// 失活实体
        /// </summary>
        /// <param name="entity">实体</param>
        void InactivateEntity(IEntity entity);
        /// <summary>
        /// 失活实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="userData">用户自定义数据</param>
        void InactivateEntity(IEntity entity, object userData);
        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityId">实体Id</param>
        /// <param name="entityGroupName">实体组名称</param>
        /// <param name="entityHelper">实体辅助器</param>
        /// <param name="userData">用户自定义信息</param>
        /// <returns>实体</returns>
        IEntity CreateEntity(Type entityType, int entityId, string entityGroupName, object userData);
        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityId">实体Id</param>
        /// <param name="entityGroup">实体组</param>
        /// <param name="entityHelper">实体辅助器</param>
        /// <param name="userData">用户自定义信息</param>
        /// <returns>实体</returns>
        IEntity CreateEntity(Type entityType, int entityId, IEntityGroup entityGroup, object userData);
        /// <summary>
        /// 注册实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="entityGroupName">实体组名称</param>
        /// <param name="userData">用户自定义信息</param>
        void RegisterEntity(IEntity entity, string entityGroupName, object userData);
        /// <summary>
        /// 注册实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="entityGroup">实体组</param>
        /// <param name="userData">用户自定义信息</param>
        void RegisterEntity(IEntity entity, IEntityGroup entityGroup, object userData);
        /// <summary>
        /// 回收实体
        /// </summary>
        /// <param name="entityId">实体Id</param>
        void RecycleEntity(int entityId);
        /// <summary>
        /// 回收实体
        /// </summary>
        /// <param name="entity">实体</param>
        void RecycleEntity(IEntity entity);
        /// <summary>
        /// 实体是否启用
        /// </summary>
        /// <param name="entityId">实体Id</param>
        /// <returns>是否启用</returns>
        bool IsActived(int entityId);
        /// <summary>
        /// 实体是否启用
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>是否启用</returns>
        bool IsActived(IEntity entity);
        TComponent AddComponent<TComponent>(int entityId) where TComponent : class, IComponent;
        IComponent AddComponent(int entityId, Type componentType);
        TComponent AddComponent<TComponent>(IEntity entity) where TComponent : class, IComponent;
        IComponent AddComponent(IEntity entity, Type componentType);
        void RemoveComponent<TComponent>(int entityId);
        void RemoveComponent(int entityId, Type componentType);
        void RemoveComponent<TComponent>(IEntity entity);
        void RemoveComponent(IEntity entity, Type componentType);
        TComponent GetComponent<TComponent>(int entityId) where TComponent : class, IComponent;
        IComponent GetComponent(int entityId, Type componentType);
        TComponent GetComponent<TComponent>(IEntity entity) where TComponent : class, IComponent;
        IComponent GetComponent(IEntity entity, Type componentType);
    }
}
