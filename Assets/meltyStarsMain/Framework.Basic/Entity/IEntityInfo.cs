namespace KuusouEngine.EngineBasic.Entity
{
    /// <summary>
    /// 实体信息接口
    /// </summary>
    public interface IEntityInfo
    {
        IEntity Entity
        {
            get;
        }
        IEntity ParentEntity
        {
            get;
            set;
        }
        IEntityInfo ParentEntityInfo
        {
            get;
            set;
        }
        EntityUpdateMode UpdateMode
        {
            get;
            set;
        }
        int ChildEntityCount
        {
            get;
        }
    }
}
