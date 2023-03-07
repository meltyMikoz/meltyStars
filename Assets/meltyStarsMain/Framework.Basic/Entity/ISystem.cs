namespace KuusouEngine.EngineBasic.Entity
{
    public interface ISystem
    {
        /// <summary>
        /// 实体初始化
        /// </summary>
        /// <param name="entityId">实体实例Id</param>
        /// <param name="entity">实体实例</param>
        /// <param name="entityGroup">实体组</param>
        /// <param name="entityInfo">实体信息</param>
        /// <param name="userData">用户自定义信息</param>
        void OnInit(int entityId, IEntity entity, IEntityGroup entityGroup, IEntityInfo entityInfo, object userData);
        /// <summary>
        /// 实体销毁(回收)
        /// </summary>
        /// <param name="entity">实体实例</param>
        /// <param name="isShutDown">是否为实体管理器关闭时触发</param>
        void OnRecycle(IEntity entity, bool isShutDown);
        /// <summary>
        /// 实体激活
        /// </summary>
        /// <param name="entity">实体实例</param>
        /// <param name="userData">用户自定义数据</param>
        void OnActivate(IEntity entity, object userData);
        /// <summary>
        /// 实体失活
        /// </summary>
        /// <param name="entity">实体实例</param>
        /// <param name="userData">用户自定义数据</param>
        void OnInactivate(IEntity entity, object userData);
        /// <summary>
        /// 实体被附加子实体
        /// </summary>
        /// <param name="entity">实体实例</param>
        /// <param name="childEntity">附加的子实体</param>
        /// <param name="userData">用户自定义数据</param>
        void OnAttached(IEntity entity, IEntity childEntity, object userData);
        /// <summary>
        /// 实体被解除子实体
        /// </summary>
        /// <param name="entity">实体实例</param>
        /// <param name="childEntity">附加的子实体</param>
        /// <param name="userData">用户自定义数据</param>
        void OnDetached(IEntity entity, IEntity childEntity, object userData);
        /// <summary>
        /// 实体附加到父实体
        /// </summary>
        /// <param name="entity">实体实例</param>
        /// <param name="parentEntity">附加的父实体</param>
        /// <param name="userData">用户自定义数据</param>
        void OnAttachTo(IEntity entity, IEntity parentEntity, object userData);
        /// <summary>
        /// 实体解除附加到父实体
        /// </summary>
        /// <param name="entity">实体实例</param>
        /// <param name="parentEntity">附加的父实体</param>
        /// <param name="userData">用户自定义数据</param>
        void OnDetachFrom(IEntity entity, IEntity parentEntity, object userData);
        /// <summary>
        /// 实体轮询
        /// </summary>
        /// <param name="entity">实体实例</param>
        /// <param name="elapseFrequency">时间流逝频率(以秒为单位)</param>
        /// <param name="elapseFrequencyReally">真实时间流逝频率(以秒为单位)</param>
        void OnUpdate(IEntity entity, float elapseFrequency, float elapseFrequencyReally);
    }
    public abstract class ISystem<TEntity> : ISystem where TEntity : class, IEntity
    {
        /// <summary>
        /// 实体初始化
        /// </summary>
        /// <param name="entityId">实体实例Id</param>
        /// <param name="entity">实体实例</param>
        /// <param name="entityGroup">实体组</param>
        /// <param name="entityInfo">实体信息</param>
        /// <param name="userData">用户自定义信息</param>
        protected abstract void OnInit(int entityId, TEntity entity, IEntityGroup entityGroup, IEntityInfo entityInfo, object userData);
        /// <summary>
        /// 实体销毁(回收)
        /// </summary>
        /// <param name="entity">实体实例</param>
        /// <param name="isShutDown">是否为实体管理器关闭时触发</param>
        protected abstract void OnRecycle(TEntity entity, bool isShutDown);
        /// <summary>
        /// 实体激活
        /// </summary>
        /// <param name="entity">实体实例</param>
        /// <param name="userData">用户自定义数据</param>
        protected abstract void OnActivate(TEntity entity, object userData);
        /// <summary>
        /// 实体失活
        /// </summary>
        /// <param name="entity">实体实例</param>
        /// <param name="userData">用户自定义数据</param>
        protected abstract void OnInactivate(TEntity entity, object userData);
        /// <summary>
        /// 实体被附加子实体
        /// </summary>
        /// <param name="entity">实体实例</param>
        /// <param name="childEntity">附加的子实体</param>
        /// <param name="userData">用户自定义数据</param>
        protected abstract void OnAttached(TEntity entity, IEntity childEntity, object userData);
        /// <summary>
        /// 实体被解除子实体
        /// </summary>
        /// <param name="entity">实体实例</param>
        /// <param name="childEntity">附加的子实体</param>
        /// <param name="userData">用户自定义数据</param>
        protected abstract void OnDetached(TEntity entity, IEntity childEntity, object userData);
        /// <summary>
        /// 实体附加到父实体
        /// </summary>
        /// <param name="entity">实体实例</param>
        /// <param name="parentEntity">附加的父实体</param>
        /// <param name="userData">用户自定义数据</param>
        protected abstract void OnAttachTo(TEntity entity, IEntity parentEntity, object userData);
        /// <summary>
        /// 实体解除附加到父实体
        /// </summary>
        /// <param name="entity">实体实例</param>
        /// <param name="parentEntity">附加的父实体</param>
        /// <param name="userData">用户自定义数据</param>
        protected abstract void OnDetachFrom(TEntity entity, IEntity parentEntity, object userData);
        /// <summary>
        /// 实体轮询
        /// </summary>
        /// <param name="entity">实体实例</param>
        /// <param name="elapseFrequency">时间流逝频率(以秒为单位)</param>
        /// <param name="elapseFrequencyReally">真实时间流逝频率(以秒为单位)</param>
        protected abstract void OnUpdate(TEntity entity, float elapseFrequency, float elapseFrequencyReally);

        public void OnInit(int entityId, IEntity entity, IEntityGroup entityGroup, IEntityInfo entityInfo, object userData)
        {
            entity.Id = entityId;
            entity.EntityGroup = entityGroup;
            entity.EntityInfo = entityInfo;
            OnInit(entityId, entity as TEntity, entityGroup, entityInfo, userData);
        }

        public void OnRecycle(IEntity entity, bool isShutDown)
        {
            OnRecycle(entity as TEntity, isShutDown);
        }

        public void OnActivate(IEntity entity, object userData)
        {
            OnActivate(entity as TEntity, userData);
        }

        public void OnInactivate(IEntity entity, object userData)
        {
            OnInactivate(entity as TEntity, userData);
        }

        public void OnAttached(IEntity entity, IEntity childEntity, object userData)
        {
            OnAttached(entity as TEntity, childEntity, userData);
        }

        public void OnDetached(IEntity entity, IEntity childEntity, object userData)
        {
            OnDetached(entity as TEntity, childEntity, userData);
        }

        public void OnAttachTo(IEntity entity, IEntity parentEntity, object userData)
        {
            OnAttachTo(entity as TEntity, parentEntity, userData);
        }

        public void OnDetachFrom(IEntity entity, IEntity parentEntity, object userData)
        {
            OnDetachFrom(entity as TEntity, parentEntity, userData);
        }

        public void OnUpdate(IEntity entity, float elapseFrequency, float elapseFrequencyReally)
        {
            OnUpdate(entity as TEntity, elapseFrequency, elapseFrequencyReally);
        }
    }
}
