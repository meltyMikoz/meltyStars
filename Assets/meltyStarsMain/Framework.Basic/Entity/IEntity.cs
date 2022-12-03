namespace MeltyStars.FrameworkBasic.Entity
{
    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 实体名称
        /// </summary>
        /// <value></value>
        string Name
        {
            get;
        }
        /// <summary>
        /// 实体完整名称
        /// </summary>
        /// <value></value>
        string FullName
        {
            get;
        }
        /// <summary>
        /// 实体实例Id
        /// </summary>
        /// <value></value>
        int InstanceId
        {
            get;
        }
        /// <summary>
        /// 实体所属实体组
        /// </summary>
        /// <value></value>
        IEntityGroup EntityGroup
        {
            get;
        }
        /// <summary>
        /// 实体是否激活(启用)
        /// </summary>
        /// <value></value>
        bool Enabled
        {
            get;
        }
        /// <summary>
        /// 实体初始化
        /// </summary>
        /// <param name="entityInstanceId">实体实例Id</param>
        /// <param name="userData">用户自定义数据</param>
        void OnInit(int entityInstanceId, object userData);
        /// <summary>
        /// 实体回收
        /// </summary>
        void OnRecycle();
        /// <summary>
        /// 实体启用
        /// </summary>
        /// <param name="userData">用户自定义数据</param>
        void OnEnable(object userData);
        /// <summary>
        /// 实体关闭
        /// </summary>
        /// <param name="managerShutDown">是否为实体管理器关闭时触发</param>
        /// <param name="userData">用户自定义数据</param>
        void OnDisable(bool managerShutDown, object userData);
        /// <summary>
        /// 实体被附加子实体
        /// </summary>
        /// <param name="childEntity">附加的子实体</param>
        /// <param name="userData">用户自定义数据</param>
        void OnAttached(IEntity childEntity, object userData);
        /// <summary>
        /// 实体被解除子实体
        /// </summary>
        /// <param name="childEntity">附加的子实体</param>
        /// <param name="userData">用户自定义数据</param>
        void OnDetached(IEntity childEntity, object userData);
        /// <summary>
        /// 实体附加到父实体
        /// </summary>
        /// <param name="parentEntity">附加的父实体</param>
        /// <param name="userData">用户自定义数据</param>
        void OnAttachTo(IEntity parentEntity, object userData);
        /// <summary>
        /// 实体解除附加到父实体
        /// </summary>
        /// <param name="parentEntity">附加的父实体</param>
        /// <param name="userData">用户自定义数据</param>
        void OnDetachFrom(IEntity parentEntity, object userData);
        /// <summary>
        /// 实体轮询
        /// </summary>
        /// <param name="elapseFrequency">时间流逝频率(以秒为单位)</param>
        /// <param name="elapseFrequencySteady">真实时间流逝频率(以秒为单位)</param>
        void OnUpdate(float elapseFrequency, float elapseFrequencyReally);
    }
}
