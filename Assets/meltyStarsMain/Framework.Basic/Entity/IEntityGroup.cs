using System;
using System.Collections.Generic;
namespace MeltyStars.FrameworkBasic.Entity
{
    public interface IEntityGroup
    {
        /// <summary>
        /// 实体组名称
        /// </summary>
        /// <value></value>
        string Name
        {
            get;
        }
        /// <summary>
        /// 实体组实体数量
        /// </summary>
        /// <value></value>
        int EntityCount
        {
            get;
        }
        /// <summary>
        /// 实体组是否拥有实体
        /// </summary>
        /// <param name="entityId">实体Id</param>
        /// <returns></returns>
        bool HasEntity(int entityId);
        /// <summary>
        /// 实体组获取实体
        /// </summary>
        /// <param name="entityId">实体Id</param>
        /// <returns></returns>
        IEntity GetEntity(int entityId);
        /// <summary>
        /// 实体组获取实体集合
        /// </summary>
        /// <param name="inherit">是否获取继承类型</param>
        /// <typeparam name="T">实体泛型类型</typeparam>
        /// <returns></returns>
        T[] GetEntities<T>(bool inherit = false) where T : IEntity;
        /// <summary>
        /// 实体组获取实体集合
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="inherit">是否获取继承类型</param>
        /// <returns></returns>
        IEntity[] GetEntities(Type entityType, bool inherit = false);
        /// <summary>
        /// 实体组获取所有实体
        /// </summary>
        /// <returns></returns>
        IEntity[] GetAllEntities();
        /// <summary>
        /// 实体组获取所有实体
        /// </summary>
        /// <param name="results">目标容器</param>
        void GetAllEntities(out List<IEntity> results);
    }
}