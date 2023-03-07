namespace KuusouEngine.EngineBasic.Entity
{
    /// <summary>
    /// 实体组辅助器接口
    /// </summary>
    public interface IEntityGroupHelper
    {
        /// <summary>
        /// 创建实体组
        /// </summary>
        /// <param name="entityGroupName"></param>
        void CreateEntityGroup(string entityGroupName);
        /// <summary>
        /// 实体组添加实体
        /// </summary>
        /// <param name="entity">实体</param>
        void AddEntity(string entityGroupName, IEntity entity);
        /// <summary>
        /// 实体组移除实体
        /// </summary>
        /// <param name="entity">实体</param>
        void RemoveEntity(string entityGroupName, IEntity entity);
    }
}
