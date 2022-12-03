namespace MeltyStars.FrameworkBasic.Entity
{
    /// <summary>
    /// 实体管理器接口
    /// </summary>
    public interface IEntityManager
    {
        int EntityCount
        {
            get;
        }
        int EntityGroupCount
        {
            get;
        }
        bool HasEntity(int entityId);
        IEntity GetEntity(int entityId);
    }
}
