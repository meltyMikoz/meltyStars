using System;

namespace KuusouEngine.EngineBasic.Fsm
{
    /// <summary>
    /// 有限状态机的基类
    /// </summary>
    public abstract class AFsm
    {
        private string _name;
        public AFsm()
        {
            this._name = string.Empty;
        }
        /// <summary>
        /// 有限状态机名称
        /// </summary>
        /// <value></value>
        public string Name
        {
            get
            {
                return this._name;
            }
            protected set
            {
                this._name = value ?? string.Empty;
            }
        }
        /// <summary>
        /// 有限状态机持有者类型
        /// </summary>
        /// <value></value>
        public abstract Type OwnerType
        {
            get;
        }
        /// <summary>
        /// 有限状态机状态数量
        /// </summary>
        /// <value></value>
        public abstract int StateCount
        {
            get;
        }
        /// <summary>
        /// 有限状态机是否正在运行
        /// </summary>
        /// <value></value>
        public abstract bool IsRunning
        {
            get;
        }
        /// <summary>
        /// 有限状态机是否被销毁
        /// </summary>
        /// <value></value>
        public abstract bool IsDestroyed
        {
            get;
        }
        /// <summary>
        /// 有限状态机当前状态名称
        /// </summary>
        /// <value></value>
        public abstract string CurrentStateName
        {
            get;
        }
        /// <summary>
        /// 有限状态机当前状态保持时间
        /// </summary>
        /// <value></value>
        public abstract float CurrentStateTime
        {
            get;
        }
        /// <summary>
        /// 有限状态机轮询
        /// </summary>
        public abstract void Update();
        /// <summary>
        /// 关闭有限状态机
        /// </summary>
        public abstract void ShutDown();
    }
}
