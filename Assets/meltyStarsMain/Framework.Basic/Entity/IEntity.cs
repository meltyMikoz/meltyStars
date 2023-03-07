namespace KuusouEngine.EngineBasic.Entity
{
    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 实体实例Id
        /// </summary>
        /// <value></value>
        int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 实体所属实体组
        /// </summary>
        /// <value></value>
        IEntityGroup EntityGroup
        {
            get;
            set;
        }
        IEntityInfo EntityInfo
        {
            get;
            set;
        }
    }
}
