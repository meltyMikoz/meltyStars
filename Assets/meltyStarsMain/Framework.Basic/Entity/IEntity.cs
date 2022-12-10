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
        }
        /// <summary>
        /// 实体辅助器
        /// </summary>
        /// <value></value>
        IEntityHelper EntityHelper
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
        /// 实体初始化
        /// </summary>
        /// <param name="entityInstanceId">实体实例Id</param>
        /// <param name="userData">用户自定义数据</param>
        void OnInit(int entityId, IEntityHelper entityHelper, object userData);
        /// <summary>
        /// 实体销毁(回收)
        /// </summary>
        /// <param name="isShutDown">是否为实体管理器关闭时触发</param>
        void OnDestroy(bool isShutDown);
        /// <summary>
        /// 实体启用
        /// </summary>
        /// <param name="userData">用户自定义数据</param>
        void OnEnable(object userData);
        /// <summary>
        /// 实体关闭
        /// </summary>
        /// <param name="userData">用户自定义数据</param>
        void OnDisable(object userData);
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
        /// <summary>
        /// 实体设置实体辅助器
        /// </summary>
        /// <param name="entityHelper">实体辅助器</param>
        void SetEntityHelper(IEntityHelper entityHelper);
    }
}
