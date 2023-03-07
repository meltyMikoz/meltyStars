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
        }
        EntityUpdateMode UpdateMode
        {
            get;
            set;
        }
        IComponent[] Components 
        { 
            get; 
        }
        int ChildEntityCount
        {
            get;
        }
        void Update(float elapseFrequency, float elapseFrequencyReally);
    }
}
